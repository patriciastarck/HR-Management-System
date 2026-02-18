using FluentValidation;
using HR_Management_System.Application.Dtos;
using HR_Management_System.Infrastructure.Repositories;

namespace HR_Management_System.Application.Validators;

public class UserRequestDtoValidator : AbstractValidator<UserRequestDto>
{
    // O construtor recebe o 'repository' (banco de dados) para fazer consultas
    public UserRequestDtoValidator(ISystemUserRepository repository)
    {
        // Define as regras para a propriedade 'Login' do DTO
        RuleFor(x => x.Login)
            // 1. Verifica se o campo não está nulo, vazio ou só com espaços
            .NotEmpty().WithMessage("O Login é obrigatório.")

            // 2. Impõe um limite mínimo de 4 letras para o nome de usuário
            .MinimumLength(4).WithMessage("O Login deve ter no mínimo 4 caracteres.")

            // 3. Regra customizada: usa C# puro para garantir que não existam espaços no meio do texto
            .Must(login => !login.Contains(" ")).WithMessage("O Login não pode conter espaços.")

            // 4. Regra assíncrona: ela "espera" o banco de dados responder se o login já existe
            .MustAsync(async (login, cancellation) =>
            {
                // Chama o banco. Se retornar true (existe), o '!' inverte para false e gera o erro
                return !await repository.LoginExists(login);
            }).WithMessage("Este login já está em uso.");

            // Agora definimos as regras para a propriedade 'Password' (Senha)
            RuleFor(x => x.Password)
            // 1. Garante que o usuário digitou uma senha
            .NotEmpty().WithMessage("A senha é obrigatória.")

            // 2. Define que a senha precisa ter 6 ou mais caracteres por segurança
            .MinimumLength(6).WithMessage("A senha deve ter pelo menos 6 caracteres.");

            // Por fim, as regras para a propriedade 'Role' (Cargo/Perfil)
            RuleFor(x => x.Role)
            // Garante que o texto não ultrapasse 50 caracteres para não quebrar o banco de dados
            .MaximumLength(50).WithMessage("O cargo não pode exceder 50 caracteres.");
    }
}
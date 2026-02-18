using FluentValidation;
using HR_Management_System.Application.Dtos;
using HR_Management_System.Infrastructure.Repositories;

namespace HR_Management_System.Application.Validators;

public class EmployeeRequestDtoValidator : AbstractValidator<EmployeeRequestDto>
{
    // O construtor deve estar dentro da classe
    public EmployeeRequestDtoValidator(IEmployeeRepository repository)
    {
        // Regra para o Nome
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O Nome é obrigatório.");

        // Regras para o CPF
        RuleFor(x => x.Cpf)
            .NotEmpty().WithMessage("O CPF é obrigatório.")
            .Length(11).WithMessage("O CPF deve ter 11 dígitos.")
            .MustAsync(async (cpf, cancellation) =>
            {
                // Inverte o resultado: se existir (true), vira false (erro)
                return !await repository.CpfExists(cpf);
            })
            .WithMessage("Este CPF já está cadastrado.");
    }
}
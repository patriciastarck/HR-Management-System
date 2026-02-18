using HR_Management_System.Application.Dtos;
using HR_Management_System.Infrastructure.Repositories; // Ajustado para o novo namespace
using Moq;
using HR_Management_System.Application.Validators;
using NUnit.Framework;

namespace HR_Management_System.Tests.Application.Validators
{
    [TestFixture]
    public class UserValidatorTest
    {
        [Test]
        public async Task Create_ShouldHaveValidationErrors_WhenLoginIsEmpty()
        {
            // 1. ARRANGE
            // Criei o dublê do repositório
            var repositoryMock = new Mock<ISystemUserRepository>();

            // Passei o objeto do mock para o validador
            var validator = new UserRequestDtoValidator(repositoryMock.Object);

            // Criamos o DTO (As propriedades Login e Password pertencem a ele!)
            var request = new UserRequestDto
            {
                Login = "", // Testando o erro de login vazio
                Password = "123",
                Role = "Admin"
            };

            // 2. ACT
            // Usamos ValidateAsync porque sua classe tem regras que acessam banco (MustAsync)
            var result = await validator.ValidateAsync(request);
                        
            // 3. ASSERT
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Errors.Any(e => e.PropertyName == "Login"), Is.True);
        }

        [Test]
        public async Task Create_ShouldHaveValidationErrors_WhenLoginIsTooShort()
        {
            // 1. ARRANGE
            // Criação do mock do repositório
            var repositoryMock = new Mock<ISystemUserRepository>();

            // Instância do validador passando o mock
            var validator = new UserRequestDtoValidator(repositoryMock.Object);

            // Criamos o DTO com um login de apenas 3 caracteres (inválido, pois o mínimo é 4)
            var request = new UserRequestDto
            {
                Login = "abc",
                Password = "password123",
                Role = "Admin"
            };

            // 2. ACT
            var result = await validator.ValidateAsync(request);

            // 3. ASSERT
            // Verifica se a validação falhou
            Assert.That(result.IsValid, Is.False);

            // Verifica se existe um erro específico para a propriedade "Login"
            // E, opcionalmente, se a mensagem de erro é a esperada
            var loginError = result.Errors.FirstOrDefault(e => e.PropertyName == "Login");
            Assert.That(loginError, Is.Not.Null);
            Assert.That(loginError.ErrorMessage, Is.EqualTo("O Login deve ter no mínimo 4 caracteres."));
        }

        [Test]
        public async Task Create_ShouldHaveValidationErrors_WhenLoginContainsSpaces()
        {
            // 1. ARRANGE
            // Criação do mock do repositório
            var repositoryMock = new Mock<ISystemUserRepository>();

            // Instância do validador passando o mock
            var validator = new UserRequestDtoValidator(repositoryMock.Object);

            // Criamos o DTO com um login contendo espaços (regra customizada .Must)
            var request = new UserRequestDto
            {
                Login = "user name",
                Password = "password123",
                Role = "Admin"
            };

            // 2. ACT
            var result = await validator.ValidateAsync(request);

            // 3. ASSERT
            // Verifica se a validação falhou
            Assert.That(result.IsValid, Is.False);

            // Verifica se existe um erro específico para a propriedade "Login" 
            // com a mensagem definida na regra customizada
            var loginError = result.Errors.FirstOrDefault(e => e.PropertyName == "Login");
            Assert.That(loginError, Is.Not.Null);
            Assert.That(loginError.ErrorMessage, Is.EqualTo("O Login não pode conter espaços."));
        }

        [Test]
        public async Task Create_ShouldHaveValidationErrors_WhenLoginAlreadyExists()
        {
            // 1. ARRANGE
            var repositoryMock = new Mock<ISystemUserRepository>();

            // CONFIGURAÇÃO DO MOCK: 
            // Dizemos que, quando o método LoginExists for chamado com "admin", ele deve retornar TRUE.
            repositoryMock
                .Setup(repo => repo.LoginExists("admin"))
                .ReturnsAsync(true);

            var validator = new UserRequestDtoValidator(repositoryMock.Object);

            // DTO com o login que o Mock dirá que já existe
            var request = new UserRequestDto
            {
                Login = "admin",
                Password = "password123",
                Role = "Admin"
            };

            // 2. ACT
            var result = await validator.ValidateAsync(request);

            // 3. ASSERT
            Assert.That(result.IsValid, Is.False);

            // Verifica se a mensagem de erro é a que definimos no MustAsync
            var loginError = result.Errors.FirstOrDefault(e => e.PropertyName == "Login");
            Assert.That(loginError, Is.Not.Null);
            Assert.That(loginError.ErrorMessage, Is.EqualTo("Este login já está em uso."));
        }

        [Test]
        public async Task Create_ShouldHaveValidationErrors_WhenPasswordIsEmpty()
        {
            // 1. ARRANGE
            var repositoryMock = new Mock<ISystemUserRepository>();

            // O validador precisa do repositório, mesmo que este teste não o utilize diretamente
            var validator = new UserRequestDtoValidator(repositoryMock.Object);

            var request = new UserRequestDto
            {
                Login = "usuario_valido",
                Password = "", // Cenário: senha vazia
                Role = "Admin"
            };

            // 2. ACT
            var result = await validator.ValidateAsync(request);

            // 3. ASSERT
            Assert.That(result.IsValid, Is.False);

            // Verifica se o erro foi capturado na propriedade Password
            var passwordError = result.Errors.FirstOrDefault(e => e.PropertyName == "Password");
            Assert.That(passwordError, Is.Not.Null);
            Assert.That(passwordError.ErrorMessage, Is.EqualTo("A senha é obrigatória."));
        }

        [Test]
        public async Task Create_ShouldHaveValidationErrors_WhenPasswordIsTooShort()
        {
            // 1. ARRANGE
            var repositoryMock = new Mock<ISystemUserRepository>();
            var validator = new UserRequestDtoValidator(repositoryMock.Object);

            // Criamos o DTO com uma senha de 5 caracteres (inválida, pois o mínimo é 6)
            var request = new UserRequestDto
            {
                Login = "usuario_valido",
                Password = "12345",
                Role = "Admin"
            };

            // 2. ACT
            var result = await validator.ValidateAsync(request);

            // 3. ASSERT
            // Verifica se a validação falhou
            Assert.That(result.IsValid, Is.False);

            // Verifica se o erro específico da senha curta foi retornado
            var passwordError = result.Errors.FirstOrDefault(e => e.PropertyName == "Password");
            Assert.That(passwordError, Is.Not.Null);
            Assert.That(passwordError.ErrorMessage, Is.EqualTo("A senha deve ter pelo menos 6 caracteres."));
        }

        [Test]
        public async Task Create_ShouldHaveValidationErrors_WhenRoleIsTooLong()
        {
            // 1. ARRANGE
            var repositoryMock = new Mock<ISystemUserRepository>();
            var validator = new UserRequestDtoValidator(repositoryMock.Object);

            // Criamos uma string com 51 caracteres para forçar o erro (o limite é 50)
            var longRole = new string('A', 51);

            var request = new UserRequestDto
            {
                Login = "usuario_valido",
                Password = "password123",
                Role = longRole
            };

            // 2. ACT
            var result = await validator.ValidateAsync(request);

            // 3. ASSERT
            Assert.That(result.IsValid, Is.False);

            // Verifica se o erro está na propriedade "Role"
            var roleError = result.Errors.FirstOrDefault(e => e.PropertyName == "Role");
            Assert.That(roleError, Is.Not.Null);
            Assert.That(roleError.ErrorMessage, Is.EqualTo("O cargo não pode exceder 50 caracteres."));
        }
    }
}
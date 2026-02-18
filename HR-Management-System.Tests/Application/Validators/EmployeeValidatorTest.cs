using HR_Management_System.Application.Dtos;
using HR_Management_System.Infrastructure.Repositories;
using HR_Management_System.Application.Validators;
using Moq;
using NUnit.Framework;

namespace HR_Management_System.Tests.Application.Validators
{
    [TestFixture]
    public class EmployeeValidatorTest
    {
        private Mock<IEmployeeRepository> _repositoryMock;
        private EmployeeRequestDtoValidator _validator;

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<IEmployeeRepository>();
            _validator = new EmployeeRequestDtoValidator(_repositoryMock.Object);
        }

        [Test]
        public async Task Create_ShouldHaveError_WhenNameIsEmpty()
        {
            // ARRANGE
            var request = new EmployeeRequestDto { Name = "", Cpf = "12345678901" };

            // ACT
            var result = await _validator.ValidateAsync(request);

            // ASSERT
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Errors.Any(e => e.PropertyName == "Name"), Is.True);
        }

        [Test]
        public async Task Create_ShouldHaveError_WhenCpfIsInvalidLength()
        {
            // ARRANGE - CPF com 10 dígitos (inválido)
            var request = new EmployeeRequestDto { Name = "Tobias", Cpf = "1234567890" };

            // ACT
            var result = await _validator.ValidateAsync(request);

            // ASSERT
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Errors.Any(e => e.ErrorMessage.Contains("11 dígitos")), Is.True);
        }

        [Test]
        public async Task Create_ShouldHaveError_WhenCpfAlreadyExists()
        {
            // ARRANGE
            var cpfExistente = "11122233344";
            _repositoryMock.Setup(repo => repo.CpfExists(cpfExistente)).ReturnsAsync(true);

            var request = new EmployeeRequestDto { Name = "Tobias", Cpf = cpfExistente };

            // ACT
            var result = await _validator.ValidateAsync(request);

            // ASSERT
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Errors.Any(e => e.ErrorMessage.Contains("já está cadastrado")), Is.True);
        }
    }
}
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using HR_Management_System.Controllers;
using HR_Management_System.Application.Services;
using HR_Management_System.Application.Dtos;

namespace HR_Management_System.Tests.Controllers
{
    [TestFixture]
    public class EmployeesControllerTest
    {
        private Mock<IEmployeeService> _serviceMock;
        private Mock<IValidator<EmployeeRequestDto>> _validatorMock;
        private EmployeesController _controller;

        [SetUp]
        public void SetUp()
        {
            // 1. Instancia os Mocks
            _serviceMock = new Mock<IEmployeeService>();
            _validatorMock = new Mock<IValidator<EmployeeRequestDto>>();

            // 2. Instancia o Controller injetando os Mocks
            _controller = new EmployeesController(_serviceMock.Object, _validatorMock.Object);

            // 3. Configura o contexto do controller (evita erros nulos em CreatedAtAction/ValidationProblem)
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
        }

        [Test]
        public async Task Create_ShouldReturnCreatedAtAction_WithValidData()
        {
            // ARRANGE
            var request = new EmployeeRequestDto { Name = "Tobias", Cpf = "12345678901" };
            var response = new EmployeeResponseDto { Id = 1, Name = "Tobias", Cpf = "12345678901" };

            _validatorMock.Setup(v => v.ValidateAsync(request, default))
                          .ReturnsAsync(new ValidationResult());

            _serviceMock.Setup(s => s.CreateAsync(request)).ReturnsAsync(response);

            // ACT
            var result = await _controller.Create(request);

            // ASSERT
            var createdResult = result as CreatedAtActionResult;
            Assert.That(createdResult, Is.Not.Null);
            Assert.That(createdResult.ActionName, Is.EqualTo("GetById"));
            Assert.That(((EmployeeResponseDto)createdResult.Value).Name, Is.EqualTo("Tobias"));
        }

        [Test]
        public async Task Create_ShouldReturnValidationProblem_WhenDataIsInvalid()
        {
            // Arrange
            var dto = new EmployeeRequestDto
            {
                Name = "",
                Cpf = "123"
            };

            _validatorMock
                .Setup(v => v.ValidateAsync(It.IsAny<EmployeeRequestDto>(), default))
                .ReturnsAsync(new ValidationResult(
                    new List<ValidationFailure>
                    {
                        new("Name", "Name is required")
                    }
                ));

            // Act
            var result = await _controller.Create(dto);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.That(objectResult, Is.Not.Null);
            Assert.That(objectResult.StatusCode ?? 400, Is.EqualTo(400));
        }

        [Test]
        public async Task GetById_ShouldReturnNotFound_WhenEmployeeDoesNotExist()
        {
            // ARRANGE
            _serviceMock.Setup(s => s.GetByIdAsync(999)).ReturnsAsync((EmployeeResponseDto?)null);

            // ACT
            var result = await _controller.GetById(999);

            // ASSERT
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }
}

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
            _serviceMock   = new Mock<IEmployeeService>();
            _validatorMock = new Mock<IValidator<EmployeeRequestDto>>();
            _controller    = new EmployeesController(_serviceMock.Object, _validatorMock.Object);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
        }

        [Test]
        public async Task Create_ShouldReturnCreatedAtAction_WithValidData()
        {
            // ARRANGE
            var request  = new EmployeeRequestDto { Name = "Tobias", Cpf = "12345678901" };
            var response = new EmployeeResponseDto { Id = 1, Name = "Tobias", Cpf = "12345678901" };

            _validatorMock.Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new ValidationResult());

            _serviceMock.Setup(s => s.CreateAsync(request))
                .ReturnsAsync(response);

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
            // ARRANGE
            var request = new EmployeeRequestDto { Name = "", Cpf = "invalid_cpf" };

            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("Name", "Name is required."),
                new ValidationFailure("Cpf", "Cpf is invalid.")
            };

            _validatorMock.Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new ValidationResult(validationFailures));

            // ACT
            var result = await _controller.Create(request);

            // ASSERT
            Assert.That(result, Is.Not.InstanceOf<CreatedAtActionResult>());
            Assert.That(result, Is.Not.InstanceOf<OkObjectResult>());

            var objectResult = result as ObjectResult;
            Assert.That(objectResult, Is.Not.Null);
            Assert.That(objectResult.Value, Is.InstanceOf<ValidationProblemDetails>());

            var validationProblem = objectResult.Value as ValidationProblemDetails;
            Assert.That(validationProblem!.Errors.ContainsKey("Name"));
            Assert.That(validationProblem.Errors["Name"], Does.Contain("Name is required."));
            Assert.That(validationProblem.Errors.ContainsKey("Cpf"));
            Assert.That(validationProblem.Errors["Cpf"], Does.Contain("Cpf is invalid."));
        }

        [Test]
        public async Task GetById_ShouldReturnNotFound_WhenEmployeeDoesNotExist()
        {
            // ARRANGE
            _serviceMock.Setup(s => s.GetByIdAsync(999))
                .ReturnsAsync((EmployeeResponseDto?)null);

            // ACT
            var result = await _controller.GetById(999);

            // ASSERT
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }
}
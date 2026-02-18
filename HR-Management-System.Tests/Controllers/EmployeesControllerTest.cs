using FluentValidation;
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

    [Test]
    public async Task Create_ShouldReturnCreatedAtAction_WithValidData()
    {
      // ARRANGE
      var request = new EmployeeRequestDto { Name = "Tobias", Cpf = "12345678901" };
      var response = new EmployeeResponseDto { Id = 1, Name = "Tobias", Cpf = "12345678901" };


      _validatorMock.Setup(v => v.ValidateAsync(request, default))
              .ReturnsAsync(new FluentValidation.Results.ValidationResult()); // No validation errors
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
      // ARRANGE
      var request = new EmployeeRequestDto { Name = "", Cpf = "invalid_cpf" }; // Invalid data
      var validationFailures = new List<FluentValidation.Results.ValidationFailure>
          {
            new FluentValidation.Results.ValidationFailure("Name", "Name is required."),
            new FluentValidation.Results.ValidationFailure("Cpf", "Cpf is invalid.")
          };

      _validatorMock.Setup(v => v.ValidateAsync(request, default))
              .ReturnsAsync(new FluentValidation.Results.ValidationResult(validationFailures));

      // ACT
      var result = await _controller.Create(request);

      // ASSERT
      var badRequestResult = result as ObjectResult;
      Assert.That(badRequestResult, Is.Not.Null);
      Assert.That(badRequestResult.StatusCode, Is.EqualTo(400)); // ValidationProblem returns 400
      Assert.That(badRequestResult.Value, Is.InstanceOf<ValidationProblemDetails>());

      var validationProblem = badRequestResult.Value as ValidationProblemDetails;
      Assert.That(validationProblem.Errors.ContainsKey("Name"));
      Assert.That(validationProblem.Errors["Name"], Does.Contain("Name is required."));
      Assert.That(validationProblem.Errors.ContainsKey("Cpf"));
      Assert.That(validationProblem.Errors["Cpf"], Does.Contain("Cpf is invalid."));
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
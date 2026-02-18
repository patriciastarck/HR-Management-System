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

        [SetUp]
        public void Setup()
        {
            _serviceMock = new Mock<IEmployeeService>();
            _validatorMock = new Mock<IValidator<EmployeeRequestDto>>();
            _controller = new EmployeesController(_serviceMock.Object, _validatorMock.Object);
        }

        [Test]
        public async Task Create_ShouldReturnCreatedAtAction_WithValidData()
        {
            // ARRANGE
            var request = new EmployeeRequestDto { Name = "Tobias", Cpf = "12345678901" };
            var response = new EmployeeResponseDto { Id = 1, Name = "Tobias", Cpf = "12345678901" };

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
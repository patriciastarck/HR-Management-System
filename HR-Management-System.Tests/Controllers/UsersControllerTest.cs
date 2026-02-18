using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using HR_Management_System.Controllers;
using HR_Management_System.Application.Services;
using HR_Management_System.Application.Dtos;

namespace HR_Management_System.Tests.Controllers
{
    [TestFixture]
    public class UsersControllerTest
    {
        private Mock<ISystemUserService> _serviceMock;
        private UsersController _controller;

        [SetUp]
        public void Setup()
        {
            _serviceMock = new Mock<ISystemUserService>();
            _controller = new UsersController(_serviceMock.Object);
        }

        [Test]
        public async Task Create_ShouldReturnCreatedAtAction_WhenUserIsCreated()
        {
            // ARRANGE
            var request = new UserRequestDto
            {
                Login = "usuario",
                Password = "123456",
                Role = "Admin"
            };

            var response = new UserResponseDto
            {
                Id = 1,
                Login = "usuario",
                Role = "Admin"
            };

            _serviceMock
                .Setup(s => s.CreateAsync(request))
                .ReturnsAsync(response);

            // ACT
            var result = await _controller.Create(request);

            // ASSERT
            var createdResult = result as CreatedAtActionResult;

            Assert.That(createdResult, Is.Not.Null);
            Assert.That(createdResult.ActionName, Is.EqualTo("GetById"));
            Assert.That(createdResult.Value, Is.EqualTo(response));
        }

        [Test]
        public async Task GetById_ShouldReturnOk_WhenUserExists()
        {
            // ARRANGE
            var user = new UserResponseDto
            {
                Id = 1,
                Login = "usuario",
                Role = "Admin"
            };

            _serviceMock
                .Setup(s => s.GetByIdAsync(1))
                .ReturnsAsync(user);

            // ACT
            var result = await _controller.GetById(1);

            // ASSERT
            var okResult = result as OkObjectResult;

            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.Value, Is.EqualTo(user));
        }

        [Test]
        public async Task GetById_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // ARRANGE
            _serviceMock
                .Setup(s => s.GetByIdAsync(1))
                .ReturnsAsync((UserResponseDto?)null);

            // ACT
            var result = await _controller.GetById(1);

            // ASSERT
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task GetAll_ShouldReturnOk_WithListOfUsers()
        {
            // ARRANGE
            var users = new List<UserResponseDto>
            {
                new UserResponseDto { Id = 1, Login = "user1", Role = "Admin" },
                new UserResponseDto { Id = 2, Login = "user2", Role = "User" }
            };

            _serviceMock
                .Setup(s => s.GetAllAsync())
                .ReturnsAsync(users);

            // ACT
            var result = await _controller.GetAll();

            // ASSERT
            var okResult = result as OkObjectResult;

            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.Value, Is.EqualTo(users));
        }
    }
}

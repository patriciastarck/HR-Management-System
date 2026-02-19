
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using HR_Management_System.Controllers;
using HR_Management_System.Application.Services;
using HR_Management_System.Application.Dtos;

namespace HR_Management_System.Tests.Controllers
{
    // ═══════════════════════════════════════════════════════
    // TESTES GET — EmployeesController
    // ═══════════════════════════════════════════════════════

    [TestFixture]
    public class EmployeesControllerGetTest
    {
        private Mock<IEmployeeService> _serviceMock;
        private Mock<FluentValidation.IValidator<EmployeeRequestDto>> _validatorMock;
        private EmployeesController _controller;

        [SetUp]
        public void SetUp()
        {
            _serviceMock   = new Mock<IEmployeeService>();
            _validatorMock = new Mock<FluentValidation.IValidator<EmployeeRequestDto>>();
            _controller    = new EmployeesController(_serviceMock.Object, _validatorMock.Object);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
        }

        // ─────────────────────────────────────────
        // GET /api/Employees/{id}
        // ─────────────────────────────────────────

        [Test]
        public async Task GetById_ShouldReturnOk_WhenEmployeeExists()
        {
            // ARRANGE
            var employee = new EmployeeResponseDto { Id = 1, Name = "Ana", Cpf = "12345678901" };

            _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(employee);

            // ACT
            var result = await _controller.GetById(1);

            // ASSERT
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult!.StatusCode, Is.EqualTo(200));

            var returned = okResult.Value as EmployeeResponseDto;
            Assert.That(returned!.Id, Is.EqualTo(1));
            Assert.That(returned.Name, Is.EqualTo("Ana"));
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

        // ─────────────────────────────────────────
        // GET /api/Employees
        // ─────────────────────────────────────────

        [Test]
        public async Task GetAll_ShouldReturnOkWithList_WhenEmployeesExist()
        {
            // ARRANGE
            var employees = new List<EmployeeResponseDto>
            {
                new EmployeeResponseDto { Id = 1, Name = "Ana",  Cpf = "11111111111" },
                new EmployeeResponseDto { Id = 2, Name = "João", Cpf = "22222222222" }
            };

            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(employees);

            // ACT
            var result = await _controller.GetAll();

            // ASSERT
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult!.StatusCode, Is.EqualTo(200));

            var returned = okResult.Value as IEnumerable<EmployeeResponseDto>;
            Assert.That(returned, Is.Not.Null);
            Assert.That(returned!.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetAll_ShouldReturnOkWithEmptyList_WhenNoEmployeesExist()
        {
            // ARRANGE
            _serviceMock.Setup(s => s.GetAllAsync())
                .ReturnsAsync(new List<EmployeeResponseDto>());

            // ACT
            var result = await _controller.GetAll();

            // ASSERT
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var returned = okResult!.Value as IEnumerable<EmployeeResponseDto>;
            Assert.That(returned!.Count(), Is.EqualTo(0));
        }
    }

    // ═══════════════════════════════════════════════════════
    // TESTES GET — UsersController
    // ═══════════════════════════════════════════════════════

    [TestFixture]
    public class UsersControllerGetTest
    {
        private Mock<ISystemUserService> _serviceMock;
        private UsersController _controller;

        [SetUp]
        public void SetUp()
        {
            _serviceMock = new Mock<ISystemUserService>();
            _controller  = new UsersController(_serviceMock.Object);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
        }

        // ─────────────────────────────────────────
        // GET /api/Users/{id}
        // ─────────────────────────────────────────

        [Test]
        public async Task GetById_ShouldReturnOk_WhenUserExists()
        {
            // ARRANGE
            var user = new UserResponseDto { Id = 1, Login = "joao123", Role = "admin" };

            _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(user);

            // ACT
            var result = await _controller.GetById(1);

            // ASSERT
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult!.StatusCode, Is.EqualTo(200));

            var returned = okResult.Value as UserResponseDto;
            Assert.That(returned!.Id, Is.EqualTo(1));
            Assert.That(returned.Login, Is.EqualTo("joao123"));
        }

        [Test]
        public async Task GetById_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // ARRANGE
            _serviceMock.Setup(s => s.GetByIdAsync(999)).ReturnsAsync((UserResponseDto?)null);

            // ACT
            var result = await _controller.GetById(999);

            // ASSERT
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        // ─────────────────────────────────────────
        // GET /api/Users
        // ─────────────────────────────────────────

        [Test]
        public async Task GetAll_ShouldReturnOkWithList_WhenUsersExist()
        {
            // ARRANGE
            var users = new List<UserResponseDto>
            {
                new UserResponseDto { Id = 1, Login = "joao123", Role = "admin" },
                new UserResponseDto { Id = 2, Login = "maria456", Role = "user" }
            };

            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(users);

            // ACT
            var result = await _controller.GetAll();

            // ASSERT
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult!.StatusCode, Is.EqualTo(200));

            var returned = okResult.Value as IEnumerable<UserResponseDto>;
            Assert.That(returned, Is.Not.Null);
            Assert.That(returned!.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetAll_ShouldReturnOkWithEmptyList_WhenNoUsersExist()
        {
            // ARRANGE
            _serviceMock.Setup(s => s.GetAllAsync())
                .ReturnsAsync(new List<UserResponseDto>());

            // ACT
            var result = await _controller.GetAll();

            // ASSERT
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var returned = okResult!.Value as IEnumerable<UserResponseDto>;
            Assert.That(returned!.Count(), Is.EqualTo(0));
        }
    }
}
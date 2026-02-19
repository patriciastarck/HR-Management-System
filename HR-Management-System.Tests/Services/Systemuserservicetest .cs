using Moq;
using NUnit.Framework;
using HR_Management_System.Application.Services;
using HR_Management_System.Application.Dtos;
using HR_Management_System.Infrastructure.Repositories;
using HR_Management_System.Models;

namespace HR_Management_System.Tests.Services
{
  [TestFixture]
  public class SystemUserServiceTest
  {
    private Mock<ISystemUserRepository> _repositoryMock;
    private SystemUserService _service;

    [SetUp]
    public void SetUp()
    {
      _repositoryMock = new Mock<ISystemUserRepository>();
      _service = new SystemUserService(_repositoryMock.Object);
    }

    // ─────────────────────────────────────────
    // CreateAsync
    // ─────────────────────────────────────────

    [Test]
    public async Task CreateAsync_ShouldReturnUserResponseDto_WhenValidData()
    {
      // ARRANGE
      var dto = new UserRequestDto
      {
        Login = "joao123",
        Password = "senha123",
        Role = "admin"
      };

      _repositoryMock.Setup(r => r.LoginExists("joao123"))
          .ReturnsAsync(false);

      _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Systemuser>()))
          .Returns(Task.CompletedTask);

      _repositoryMock.Setup(r => r.SaveChangesAsync())
          .Returns(Task.CompletedTask);

      // ACT
      var result = await _service.CreateAsync(dto);

      // ASSERT
      Assert.That(result, Is.Not.Null);
      Assert.That(result.Login, Is.EqualTo("joao123"));
      Assert.That(result.Role, Is.EqualTo("admin"));
    }

    [Test]
    public void CreateAsync_ShouldThrowArgumentNullException_WhenDtoIsNull()
    {
      // ASSERT
      Assert.ThrowsAsync<ArgumentNullException>(async () =>
          await _service.CreateAsync(null!));
    }

    [Test]
    public void CreateAsync_ShouldThrowInvalidOperationException_WhenLoginAlreadyExists()
    {
      // ARRANGE
      var dto = new UserRequestDto
      {
        Login = "loginexistente",
        Password = "senha123",
        Role = "user"
      };

      _repositoryMock.Setup(r => r.LoginExists("loginexistente"))
          .ReturnsAsync(true); // login já existe

      // ASSERT
      Assert.ThrowsAsync<InvalidOperationException>(async () =>
          await _service.CreateAsync(dto));
    }

    [Test]
    public void CreateAsync_ShouldThrowArgumentException_WhenLoginIsTooShort()
    {
      // ARRANGE
      var dto = new UserRequestDto
      {
        Login = "ab", // menos de 4 caracteres
        Password = "senha123",
        Role = "user"
      };

      _repositoryMock.Setup(r => r.LoginExists(It.IsAny<string>()))
          .ReturnsAsync(false);

      // ASSERT
      Assert.ThrowsAsync<ArgumentException>(async () =>
          await _service.CreateAsync(dto));
    }

    // ─────────────────────────────────────────
    // GetByIdAsync
    // ─────────────────────────────────────────

    [Test]
    public async Task GetByIdAsync_ShouldReturnUser_WhenExists()
    {
      // ARRANGE
      var user = new Systemuser { Id = 1, Login = "joao123", Password = "senha", Role = "admin" };

      _repositoryMock.Setup(r => r.GetByIdAsync(1))
          .ReturnsAsync(user);

      // ACT
      var result = await _service.GetByIdAsync(1);

      // ASSERT
      Assert.That(result, Is.Not.Null);
      Assert.That(result!.Id, Is.EqualTo(1));
      Assert.That(result.Login, Is.EqualTo("joao123"));
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
    {
      // ARRANGE
      _repositoryMock.Setup(r => r.GetByIdAsync(999))
          .ReturnsAsync((Systemuser?)null);

      // ACT
      var result = await _service.GetByIdAsync(999);

      // ASSERT
      Assert.That(result, Is.Null);
    }

    // ─────────────────────────────────────────
    // UpdateAsync
    // ─────────────────────────────────────────

    [Test]
    public async Task UpdateAsync_ShouldReturnUpdatedUser_WhenExists()
    {
      // ARRANGE
      var dto = new UserRequestDto
      {
        Login = "joaoatualizado",
        Password = "novasenha",
        Role = "user"
      };

      var userRetornado = new Systemuser
      {
        Id = 1,
        Login = "joaoatualizado",
        Password = "novasenha",
        Role = "user"
      };

      _repositoryMock.Setup(r => r.UpdateAsync(1, It.IsAny<Systemuser>()))
          .ReturnsAsync(userRetornado);

      _repositoryMock.Setup(r => r.SaveChangesAsync())
          .Returns(Task.CompletedTask);

      // ACT
      var result = await _service.UpdateAsync(1, dto);

      // ASSERT
      Assert.That(result, Is.Not.Null);
      Assert.That(result!.Login, Is.EqualTo("joaoatualizado"));
    }

    [Test]
    public async Task UpdateAsync_ShouldReturnNull_WhenUserNotFound()
    {
      // ARRANGE
      var dto = new UserRequestDto
      {
        Login = "qualquer",
        Password = "senha",
        Role = "user"
      };

      _repositoryMock.Setup(r => r.UpdateAsync(999, It.IsAny<Systemuser>()))
          .ReturnsAsync((Systemuser?)null);

      // ACT
      var result = await _service.UpdateAsync(999, dto);

      // ASSERT
      Assert.That(result, Is.Null);
    }

    [Test]
    public void UpdateAsync_ShouldThrowArgumentNullException_WhenDtoIsNull()
    {
      // ASSERT
      Assert.ThrowsAsync<ArgumentNullException>(async () =>
          await _service.UpdateAsync(1, null!));
    }
  }
}
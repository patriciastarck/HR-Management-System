using Moq;
using NUnit.Framework;
using HR_Management_System.Application.Services;
using HR_Management_System.Application.Dtos;
using HR_Management_System.Infrastructure.Repositories;
using HR_Management_System.Models;

namespace HR_Management_System.Tests.Services
{
  [TestFixture]
  public class EmployeeServiceTest
  {
    private Mock<IEmployeeRepository> _repositoryMock;
    private EmployeeService _service;

    [SetUp]
    public void SetUp()
    {
      _repositoryMock = new Mock<IEmployeeRepository>();
      _service = new EmployeeService(_repositoryMock.Object);
    }

    // ─────────────────────────────────────────
    // CreateAsync
    // ─────────────────────────────────────────

    [Test]
    public async Task CreateAsync_ShouldReturnEmployeeResponseDto_WhenValidData()
    {
      var dto = new EmployeeRequestDto { Name = "Ana", Cpf = "12345678901" };

      _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Employee>())).Returns(Task.CompletedTask);
      _repositoryMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

      var result = await _service.CreateAsync(dto);

      Assert.That(result, Is.Not.Null);
      Assert.That(result.Name, Is.EqualTo("Ana"));
      Assert.That(result.Cpf, Is.EqualTo("12345678901"));
    }

    [Test]
    public void CreateAsync_ShouldThrowArgumentNullException_WhenDtoIsNull()
    {
      Assert.ThrowsAsync<ArgumentNullException>(async () =>
          await _service.CreateAsync(null!));
    }

    // ─────────────────────────────────────────
    // GetByIdAsync
    // ─────────────────────────────────────────

    [Test]
    public async Task GetByIdAsync_ShouldReturnEmployee_WhenExists()
    {
      var employee = new Employee { Id = 1, Name = "Ana", Cpf = "12345678901" };

      _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(employee);

      var result = await _service.GetByIdAsync(1);

      Assert.That(result, Is.Not.Null);
      Assert.That(result!.Id, Is.EqualTo(1));
      Assert.That(result.Name, Is.EqualTo("Ana"));
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
    {
      _repositoryMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Employee?)null);

      var result = await _service.GetByIdAsync(999);

      Assert.That(result, Is.Null);
    }

    // ─────────────────────────────────────────
    // UpdateAsync
    // ─────────────────────────────────────────

    [Test]
    public async Task UpdateAsync_ShouldReturnUpdatedEmployee_WhenExists()
    {
      var dto = new EmployeeUpdateRequestDto { Name = "Ana Atualizada", Email = "ana@email.com", Salary = 5000 };

      var employeeRetornado = new Employee { Id = 1, Name = "Ana Atualizada", Cpf = "12345678901", Email = "ana@email.com", Salary = 5000 };

      _repositoryMock.Setup(r => r.UpdateAsync(1, It.IsAny<Employee>())).ReturnsAsync(employeeRetornado);
      _repositoryMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

      var result = await _service.UpdateAsync(1, dto);

      Assert.That(result, Is.Not.Null);
      Assert.That(result!.Name, Is.EqualTo("Ana Atualizada"));
    }

    [Test]
    public async Task UpdateAsync_ShouldReturnNull_WhenEmployeeNotFound()
    {
      var dto = new EmployeeUpdateRequestDto { Name = "Qualquer" };

      _repositoryMock.Setup(r => r.UpdateAsync(999, It.IsAny<Employee>())).ReturnsAsync((Employee?)null);

      var result = await _service.UpdateAsync(999, dto);

      Assert.That(result, Is.Null);
    }

    [Test]
    public void UpdateAsync_ShouldThrowArgumentNullException_WhenDtoIsNull()
    {
      Assert.ThrowsAsync<ArgumentNullException>(async () =>
          await _service.UpdateAsync(1, null!));
    }

    // ─────────────────────────────────────────
    // DeleteAsync
    // ─────────────────────────────────────────

    [Test]
    public async Task DeleteAsync_ShouldReturnTrue_WhenEmployeeExists()
    {
      _repositoryMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

      var result = await _service.DeleteAsync(1);

      Assert.That(result, Is.True);
    }

    [Test]
    public async Task DeleteAsync_ShouldReturnFalse_WhenEmployeeNotFound()
    {
      _repositoryMock.Setup(r => r.DeleteAsync(999)).ReturnsAsync(false);

      var result = await _service.DeleteAsync(999);

      Assert.That(result, Is.False);
    }
  }
}
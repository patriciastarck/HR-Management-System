using Moq;
using NUnit.Framework;
using HR_Management_System.Application.Services;
using HR_Management_System.Application.Dtos;
using HR_Management_System.Infrastructure.Repositories;
using HR_Management_System.Models;

namespace HR_Management_System.Tests.Services
{
  [TestFixture]
  public class DepartmentServiceTest
  {
    private Mock<IDepartmentRepository> _repositoryMock;
    private DepartmentService _service;

    [SetUp]
    public void SetUp()
    {
      _repositoryMock = new Mock<IDepartmentRepository>();
      _service = new DepartmentService(_repositoryMock.Object);
    }

    // ─────────────────────────────────────────
    // CreateAsync
    // ─────────────────────────────────────────

    [Test]
    public async Task CreateAsync_ShouldReturnDepartmentResponseDto_WhenValidData()
    {
      // ARRANGE
      var dto = new DepartmentRequestDto { Name = "TI" };

      _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Department>())).Returns(Task.CompletedTask);
      _repositoryMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

      // ACT
      var result = await _service.CreateAsync(dto);

      // ASSERT
      Assert.That(result, Is.Not.Null);
      Assert.That(result.Name, Is.EqualTo("TI"));
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
    public async Task GetByIdAsync_ShouldReturnDepartment_WhenExists()
    {
      // ARRANGE
      var department = new Department { Id = 1, Name = "TI" };

      _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(department);

      // ACT
      var result = await _service.GetByIdAsync(1);

      // ASSERT
      Assert.That(result, Is.Not.Null);
      Assert.That(result!.Id, Is.EqualTo(1));
      Assert.That(result.Name, Is.EqualTo("TI"));
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
    {
      // ARRANGE
      _repositoryMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Department?)null);

      // ACT
      var result = await _service.GetByIdAsync(999);

      // ASSERT
      Assert.That(result, Is.Null);
    }

    // ─────────────────────────────────────────
    // GetAllAsync
    // ─────────────────────────────────────────

    [Test]
    public async Task GetAllAsync_ShouldReturnList_WhenDepartmentsExist()
    {
      // ARRANGE
      var departments = new List<Department>
            {
                new Department { Id = 1, Name = "TI" },
                new Department { Id = 2, Name = "RH" }
            };

      _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(departments);

      // ACT
      var result = await _service.GetAllAsync();

      // ASSERT
      Assert.That(result, Is.Not.Null);
      Assert.That(result.Count(), Is.EqualTo(2));
    }

    [Test]
    public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoDepartmentsExist()
    {
      // ARRANGE
      _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Department>());

      // ACT
      var result = await _service.GetAllAsync();

      // ASSERT
      Assert.That(result.Count(), Is.EqualTo(0));
    }

    // ─────────────────────────────────────────
    // UpdateAsync
    // ─────────────────────────────────────────

    [Test]
    public async Task UpdateAsync_ShouldReturnUpdatedDepartment_WhenExists()
    {
      // ARRANGE
      var dto = new DepartmentRequestDto { Name = "Financeiro" };

      var departmentRetornado = new Department { Id = 1, Name = "Financeiro" };

      _repositoryMock.Setup(r => r.UpdateAsync(1, It.IsAny<Department>())).ReturnsAsync(departmentRetornado);
      _repositoryMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

      // ACT
      var result = await _service.UpdateAsync(1, dto);

      // ASSERT
      Assert.That(result, Is.Not.Null);
      Assert.That(result!.Name, Is.EqualTo("Financeiro"));
    }

    [Test]
    public async Task UpdateAsync_ShouldReturnNull_WhenDepartmentNotFound()
    {
      // ARRANGE
      var dto = new DepartmentRequestDto { Name = "Qualquer" };

      _repositoryMock.Setup(r => r.UpdateAsync(999, It.IsAny<Department>())).ReturnsAsync((Department?)null);

      // ACT
      var result = await _service.UpdateAsync(999, dto);

      // ASSERT
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
    public async Task DeleteAsync_ShouldReturnTrue_WhenDepartmentExists()
    {
      // ARRANGE
      _repositoryMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

      // ACT
      var result = await _service.DeleteAsync(1);

      // ASSERT
      Assert.That(result, Is.True);
    }

    [Test]
    public async Task DeleteAsync_ShouldReturnFalse_WhenDepartmentNotFound()
    {
      // ARRANGE
      _repositoryMock.Setup(r => r.DeleteAsync(999)).ReturnsAsync(false);

      // ACT
      var result = await _service.DeleteAsync(999);

      // ASSERT
      Assert.That(result, Is.False);
    }
  }
}
using Moq;
using NUnit.Framework;
using HR_Management_System.Application.Services;
using HR_Management_System.Application.Dtos;
using HR_Management_System.Infrastructure.Repositories;
using HR_Management_System.Models;

namespace HR_Management_System.Tests.Services
{
  [TestFixture]
  public class RequestServiceTest
  {
    private Mock<IRequestRepository> _repositoryMock;
    private RequestService _service;

    [SetUp]
    public void SetUp()
    {
      _repositoryMock = new Mock<IRequestRepository>();
      _service = new RequestService(_repositoryMock.Object);
    }

    // ─────────────────────────────────────────
    // CreateAsync
    // ─────────────────────────────────────────

    [Test]
    public async Task CreateAsync_ShouldReturnRequestResponseDto_WhenValidData()
    {
      // ARRANGE
      var dto = new RequestDto
      {
        StartDate = new DateOnly(2025, 1, 10),
        EndDate = new DateOnly(2025, 1, 20),
        Status = "Pendente",
        Type = "Férias",
        EmployeeId = 1
      };

      _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Request>())).Returns(Task.CompletedTask);
      _repositoryMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

      // ACT
      var result = await _service.CreateAsync(dto);

      // ASSERT
      Assert.That(result, Is.Not.Null);
      Assert.That(result.Status, Is.EqualTo("Pendente"));
      Assert.That(result.Type, Is.EqualTo("Férias"));
      Assert.That(result.EmployeeId, Is.EqualTo(1));
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
    public async Task GetByIdAsync_ShouldReturnRequest_WhenExists()
    {
      // ARRANGE
      var request = new Request
      {
        Id = 1,
        Status = "Pendente",
        Type = "Férias",
        EmployeeId = 1
      };

      _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(request);

      // ACT
      var result = await _service.GetByIdAsync(1);

      // ASSERT
      Assert.That(result, Is.Not.Null);
      Assert.That(result!.Id, Is.EqualTo(1));
      Assert.That(result.Status, Is.EqualTo("Pendente"));
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
    {
      // ARRANGE
      _repositoryMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Request?)null);

      // ACT
      var result = await _service.GetByIdAsync(999);

      // ASSERT
      Assert.That(result, Is.Null);
    }

    // ─────────────────────────────────────────
    // GetAllAsync
    // ─────────────────────────────────────────

    [Test]
    public async Task GetAllAsync_ShouldReturnList_WhenRequestsExist()
    {
      // ARRANGE
      var requests = new List<Request>
            {
                new Request { Id = 1, Status = "Pendente", Type = "Férias",   EmployeeId = 1 },
                new Request { Id = 2, Status = "Aprovado", Type = "Licença",  EmployeeId = 2 }
            };

      _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(requests);

      // ACT
      var result = await _service.GetAllAsync();

      // ASSERT
      Assert.That(result, Is.Not.Null);
      Assert.That(result.Count(), Is.EqualTo(2));
    }

    [Test]
    public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoRequestsExist()
    {
      // ARRANGE
      _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Request>());

      // ACT
      var result = await _service.GetAllAsync();

      // ASSERT
      Assert.That(result.Count(), Is.EqualTo(0));
    }

    // ─────────────────────────────────────────
    // UpdateAsync
    // ─────────────────────────────────────────

    [Test]
    public async Task UpdateAsync_ShouldReturnUpdatedRequest_WhenExists()
    {
      // ARRANGE
      var dto = new RequestDto
      {
        Status = "Aprovado",
        Type = "Férias",
        EmployeeId = 1
      };

      var requestRetornado = new Request
      {
        Id = 1,
        Status = "Aprovado",
        Type = "Férias",
        EmployeeId = 1
      };

      _repositoryMock.Setup(r => r.UpdateAsync(1, It.IsAny<Request>())).ReturnsAsync(requestRetornado);
      _repositoryMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

      // ACT
      var result = await _service.UpdateAsync(1, dto);

      // ASSERT
      Assert.That(result, Is.Not.Null);
      Assert.That(result!.Status, Is.EqualTo("Aprovado"));
    }

    [Test]
    public async Task UpdateAsync_ShouldReturnNull_WhenRequestNotFound()
    {
      // ARRANGE
      var dto = new RequestDto { Status = "Qualquer", EmployeeId = 1 };

      _repositoryMock.Setup(r => r.UpdateAsync(999, It.IsAny<Request>())).ReturnsAsync((Request?)null);

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
    public async Task DeleteAsync_ShouldReturnTrue_WhenRequestExists()
    {
      // ARRANGE
      _repositoryMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

      // ACT
      var result = await _service.DeleteAsync(1);

      // ASSERT
      Assert.That(result, Is.True);
    }

    [Test]
    public async Task DeleteAsync_ShouldReturnFalse_WhenRequestNotFound()
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
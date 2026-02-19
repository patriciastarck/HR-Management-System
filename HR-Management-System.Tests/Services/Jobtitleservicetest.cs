using Moq;
using NUnit.Framework;
using HR_Management_System.Application.Services;
using HR_Management_System.Application.Dtos;
using HR_Management_System.Infrastructure.Repositories;
using HR_Management_System.Models;

namespace HR_Management_System.Tests.Services
{
  [TestFixture]
  public class JobTitleServiceTest
  {
    private Mock<IJobTitleRepository> _repositoryMock;
    private JobTitleService _service;

    [SetUp]
    public void SetUp()
    {
      _repositoryMock = new Mock<IJobTitleRepository>();
      _service = new JobTitleService(_repositoryMock.Object);
    }

    // ─────────────────────────────────────────
    // CreateAsync
    // ─────────────────────────────────────────

    [Test]
    public async Task CreateAsync_ShouldReturnJobTitleResponseDto_WhenValidData()
    {
      // ARRANGE
      var dto = new JobTitleRequestDto { Title = "Desenvolvedor", Description = "Dev backend" };

      _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Jobtitle>())).Returns(Task.CompletedTask);
      _repositoryMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

      // ACT
      var result = await _service.CreateAsync(dto);

      // ASSERT
      Assert.That(result, Is.Not.Null);
      Assert.That(result.Title, Is.EqualTo("Desenvolvedor"));
      Assert.That(result.Description, Is.EqualTo("Dev backend"));
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
    public async Task GetByIdAsync_ShouldReturnJobTitle_WhenExists()
    {
      // ARRANGE
      var jobTitle = new Jobtitle { Id = 1, Title = "Desenvolvedor", Description = "Dev backend" };

      _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(jobTitle);

      // ACT
      var result = await _service.GetByIdAsync(1);

      // ASSERT
      Assert.That(result, Is.Not.Null);
      Assert.That(result!.Id, Is.EqualTo(1));
      Assert.That(result.Title, Is.EqualTo("Desenvolvedor"));
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
    {
      // ARRANGE
      _repositoryMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Jobtitle?)null);

      // ACT
      var result = await _service.GetByIdAsync(999);

      // ASSERT
      Assert.That(result, Is.Null);
    }

    // ─────────────────────────────────────────
    // GetAllAsync
    // ─────────────────────────────────────────

    [Test]
    public async Task GetAllAsync_ShouldReturnList_WhenJobTitlesExist()
    {
      // ARRANGE
      var jobTitles = new List<Jobtitle>
            {
                new Jobtitle { Id = 1, Title = "Desenvolvedor" },
                new Jobtitle { Id = 2, Title = "Analista" }
            };

      _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(jobTitles);

      // ACT
      var result = await _service.GetAllAsync();

      // ASSERT
      Assert.That(result, Is.Not.Null);
      Assert.That(result.Count(), Is.EqualTo(2));
    }

    [Test]
    public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoJobTitlesExist()
    {
      // ARRANGE
      _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Jobtitle>());

      // ACT
      var result = await _service.GetAllAsync();

      // ASSERT
      Assert.That(result.Count(), Is.EqualTo(0));
    }

    // ─────────────────────────────────────────
    // UpdateAsync
    // ─────────────────────────────────────────

    [Test]
    public async Task UpdateAsync_ShouldReturnUpdatedJobTitle_WhenExists()
    {
      // ARRANGE
      var dto = new JobTitleRequestDto { Title = "Senior Dev", Description = "Dev senior" };

      var jobTitleRetornado = new Jobtitle { Id = 1, Title = "Senior Dev", Description = "Dev senior" };

      _repositoryMock.Setup(r => r.UpdateAsync(1, It.IsAny<Jobtitle>())).ReturnsAsync(jobTitleRetornado);
      _repositoryMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

      // ACT
      var result = await _service.UpdateAsync(1, dto);

      // ASSERT
      Assert.That(result, Is.Not.Null);
      Assert.That(result!.Title, Is.EqualTo("Senior Dev"));
    }

    [Test]
    public async Task UpdateAsync_ShouldReturnNull_WhenJobTitleNotFound()
    {
      // ARRANGE
      var dto = new JobTitleRequestDto { Title = "Qualquer" };

      _repositoryMock.Setup(r => r.UpdateAsync(999, It.IsAny<Jobtitle>())).ReturnsAsync((Jobtitle?)null);

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
    public async Task DeleteAsync_ShouldReturnTrue_WhenJobTitleExists()
    {
      // ARRANGE
      _repositoryMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

      // ACT
      var result = await _service.DeleteAsync(1);

      // ASSERT
      Assert.That(result, Is.True);
    }

    [Test]
    public async Task DeleteAsync_ShouldReturnFalse_WhenJobTitleNotFound()
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

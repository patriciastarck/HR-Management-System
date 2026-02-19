using HR_Management_System.Application.Dtos;
using HR_Management_System.Controllers;
using HR_Management_System.Infrastructure.Repositories;
using HR_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace HR_Management_System.Tests.Controllers;

[TestFixture]
public class TimesheetsControllerTest
{
    private Mock<ITimesheetRepository> _repositoryMock;
    private TimesheetsController _controller;

    [SetUp]
    public void Setup()
    {
        // ARRANGE GLOBAL
        _repositoryMock = new Mock<ITimesheetRepository>();
        _controller = new TimesheetsController(_repositoryMock.Object);
    }

    [Test]
    public async Task RecordTime_ShouldReturnOk_WhenRequestIsValid()
    {
        // 1. Arrange
        var dto = new TimesheetRequestDto
        {
            EmployeeId = 1,
            Date = new DateOnly(2026, 2, 19),
            EntryTime = new TimeOnly(8, 0),
            ExitTime = new TimeOnly(17, 0)
        };

        // 2. Act
        var result = await _controller.RecordTime(dto);

        // 3. Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Timesheet>()), Times.Once);
            _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        });
    }

    [Test]
    public async Task GetByEmployee_ShouldReturnMappedDtoList_WhenRecordsExist()
    {
        // 1. Arrange
        int employeeId = 1;
        var fakeLogs = new List<Timesheet>
        {
            new Timesheet
            {
                Id = 10,
                EmployeeId = employeeId,
                Date = new DateOnly(2026, 2, 19),
                Employee = new Employee { Name = "Patrício Santos" }
            }
        };

        _repositoryMock.Setup(r => r.GetByEmployeeIdAsync(employeeId))
                       .ReturnsAsync(fakeLogs);

        // 2. Act
        var result = await _controller.GetByEmployee(employeeId);

        // 3. Assert
        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);

        var responseData = okResult.Value as IEnumerable<TimesheetResponseDto>;
        Assert.That(responseData.First().EmployeeName, Is.EqualTo("Patrício Santos"));
    }

    [Test]
    public async Task GetByEmployee_ShouldReturnEmptyList_WhenEmployeeHasNoRecords()
    {
        // 1. Arrange
        int nonExistentId = 999;
        _repositoryMock.Setup(r => r.GetByEmployeeIdAsync(nonExistentId))
                       .ReturnsAsync(new List<Timesheet>());

        // 2. Act
        var result = await _controller.GetByEmployee(nonExistentId);

        // 3. Assert
        var okResult = result as OkObjectResult;
        var responseData = okResult.Value as IEnumerable<TimesheetResponseDto>;
        Assert.That(responseData, Is.Empty);
    }

    [Test]
    public async Task GetByEmployee_ShouldReturnNullEmployeeName_WhenEmployeeRelationIsNull()
    {
        // 1. Arrange
        int employeeId = 1;
        var logsWithNullEmployee = new List<Timesheet>
        {
            new Timesheet
            {
                Id = 1,
                EmployeeId = employeeId,
                Date = new DateOnly(2026, 2, 19),
                Employee = null
            }
        };

        _repositoryMock.Setup(r => r.GetByEmployeeIdAsync(employeeId))
                       .ReturnsAsync(logsWithNullEmployee);

        // 2. Act
        var result = await _controller.GetByEmployee(employeeId);

        // 3. Assert
        var okResult = result as OkObjectResult;
        var response = okResult.Value as IEnumerable<TimesheetResponseDto>;
        Assert.That(response.First().EmployeeName, Is.Null);
    }
}
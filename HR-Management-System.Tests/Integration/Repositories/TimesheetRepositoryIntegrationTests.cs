using HR_Management_System.Models;
using HR_Management_System.Infrastructure.Repositories;
using HR_Management_System.Tests.Integration;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

// Adicione o using correto para o IntegrationDbContextFactory
// Exemplo: usando HR_Management_System.Tests.Integration.Helpers;

namespace HR_Management_System.Tests.Integration;

public class TimesheetRepositoryIntegrationTests
{
    [Test]
    public async Task AddTimesheet_ShouldPersistInDatabase()
    {
        // Arrange
        using var context = IntegrationDbContextFactory.Create();
        var repository = new TimesheetRepository(context);

        var employee = new Employee
        {
            Name = "Teste",
            Cpf = "12345678900",
            Email = "teste@email.com"
        };

        context.Employees.Add(employee);
        await context.SaveChangesAsync();

        var timesheet = new Timesheet
        {
            EmployeeId = employee.Id,
            Date = DateOnly.FromDateTime(DateTime.Today),
            EntryTime = new TimeOnly(8, 0),
            ExitTime = new TimeOnly(17, 0)
        };

        // Act
        await repository.AddAsync(timesheet);
        await context.SaveChangesAsync();

        // Assert
        var saved = await context.Timesheets
            .FirstOrDefaultAsync(t => t.EmployeeId == employee.Id);

        Assert.That(saved, Is.Not.Null);
        Assert.That(saved.EmployeeId, Is.EqualTo(employee.Id));
        Assert.That(saved.Date, Is.EqualTo(timesheet.Date));
    }
}
using HR_Management_System.Models;

namespace HR_Management_System.Infrastructure.Repositories;

public interface ITimesheetRepository
{
    Task AddAsync(Timesheet timesheet);
    Task<IEnumerable<Timesheet>> GetByEmployeeIdAsync(int employeeId);
    Task SaveChangesAsync();
}
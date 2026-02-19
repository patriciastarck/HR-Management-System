using HR_Management_System.Models;
using Microsoft.EntityFrameworkCore;

namespace HR_Management_System.Infrastructure.Repositories;

public class TimesheetRepository : ITimesheetRepository
{
    private readonly RhContext _context;

    public TimesheetRepository(RhContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Timesheet timesheet)
    {
        await _context.Timesheets.AddAsync(timesheet);
    }

    public async Task<IEnumerable<Timesheet>> GetByEmployeeIdAsync(int employeeId)
    {
        return await _context.Timesheets
            .Include(t => t.Employee) // Faz o Join com a tabela de Funcionário
            .Where(t => t.EmployeeId == employeeId)
            .ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
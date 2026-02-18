using HR_Management_System.Models;
using Microsoft.EntityFrameworkCore;

namespace HR_Management_System.Infrastructure.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly RhContext _context;

    public EmployeeRepository(RhContext context)
    {
        _context = context;
    }

    public async Task<bool> CpfExists(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf)) return false;
        return await _context.Employees.AnyAsync(e => e.Cpf == cpf);
    }

    public async Task AddAsync(Employee employee)
    {
        await _context.Employees.AddAsync(employee);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<Employee?> GetByIdAsync(int id)
    {
        return await _context.Employees
            .Include(e => e.Department)
            .Include(e => e.JobTitle)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Employee>> GetAllAsync()
    {
        return await _context.Employees.ToListAsync();
    }
}
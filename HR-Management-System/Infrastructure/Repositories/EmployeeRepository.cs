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
  public async Task<Employee?> UpdateAsync(int id, Employee updatedData)
  {
    var employee = await _context.Employees.FindAsync(id);

    if (employee is null) return null;

    // Só atualiza se o valor foi enviado (não nulo)
    if (updatedData.Name is not null) employee.Name = updatedData.Name;
    if (updatedData.Email is not null) employee.Email = updatedData.Email;
    if (updatedData.Salary is not null) employee.Salary = updatedData.Salary;
    if (updatedData.IsActive is not null) employee.IsActive = updatedData.IsActive;
    if (updatedData.DepartmentId is not null) employee.DepartmentId = updatedData.DepartmentId;
    if (updatedData.JobTitleId is not null) employee.JobTitleId = updatedData.JobTitleId;

    return employee;
  }

  public async Task<bool> DeleteAsync(int id)
  {
    var employee = await _context.Employees.FindAsync(id);

    if (employee is null) return false;

    _context.Employees.Remove(employee);
    await _context.SaveChangesAsync();

    return true;
  }
}
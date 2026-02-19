using HR_Management_System.Models;
using Microsoft.EntityFrameworkCore;

namespace HR_Management_System.Infrastructure.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
  private readonly RhContext _context;

  public DepartmentRepository(RhContext context)
  {
    _context = context;
  }

  public async Task AddAsync(Department department)
  {
    await _context.Departments.AddAsync(department);
  }

  public async Task SaveChangesAsync()
  {
    await _context.SaveChangesAsync();
  }

  public async Task<Department?> GetByIdAsync(int id)
  {
    return await _context.Departments.FindAsync(id);
  }

  public async Task<IEnumerable<Department>> GetAllAsync()
  {
    return await _context.Departments.ToListAsync();
  }

  public async Task<Department?> UpdateAsync(int id, Department updatedData)
  {
    var department = await _context.Departments.FindAsync(id);

    if (department is null) return null;

    if (updatedData.Name is not null) department.Name = updatedData.Name;

    return department;
  }

  public async Task<bool> DeleteAsync(int id)
  {
    var department = await _context.Departments.FindAsync(id);

    if (department is null) return false;

    _context.Departments.Remove(department);
    await _context.SaveChangesAsync();

    return true;
  }
}
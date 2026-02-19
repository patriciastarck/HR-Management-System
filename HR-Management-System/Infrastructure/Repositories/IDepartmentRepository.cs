using HR_Management_System.Models;

namespace HR_Management_System.Infrastructure.Repositories;

public interface IDepartmentRepository
{
  Task AddAsync(Department department);
  Task SaveChangesAsync();
  Task<Department?> GetByIdAsync(int id);
  Task<IEnumerable<Department>> GetAllAsync();
  Task<Department?> UpdateAsync(int id, Department department);
  Task<bool> DeleteAsync(int id);
}
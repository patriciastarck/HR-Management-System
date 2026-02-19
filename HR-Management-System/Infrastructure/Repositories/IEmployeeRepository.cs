using System.Threading.Tasks;
using HR_Management_System.Models;

namespace HR_Management_System.Infrastructure.Repositories;

public interface IEmployeeRepository
{
  Task<bool> CpfExists(string cpf);
  Task AddAsync(Employee employee);
  Task SaveChangesAsync();
  Task<Employee?> GetByIdAsync(int id);
  Task<IEnumerable<Employee>> GetAllAsync();
  Task<Employee?> UpdateAsync(int id, Employee employee);
}
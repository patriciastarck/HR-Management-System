using System.Threading.Tasks;
using HR_Management_System.Models;

namespace HR_Management_System.Infrastructure.Repositories;

public interface ISystemUserRepository
{
  Task<bool> LoginExists(string login);
  Task AddAsync(Systemuser user);
  Task SaveChangesAsync();
  Task<Systemuser?> GetByIdAsync(int id);
  Task<IEnumerable<Systemuser>> GetAllAsync();
  Task<Systemuser?> UpdateAsync(int id, Systemuser user);
  Task<bool> DeleteAsync(int id); // ← NOVO
}
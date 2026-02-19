
using HR_Management_System.Models;

namespace HR_Management_System.Infrastructure.Repositories;

public interface IJobTitleRepository
{
  Task AddAsync(Jobtitle jobTitle);
  Task SaveChangesAsync();
  Task<Jobtitle?> GetByIdAsync(int id);
  Task<IEnumerable<Jobtitle>> GetAllAsync();
  Task<Jobtitle?> UpdateAsync(int id, Jobtitle jobTitle);
  Task<bool> DeleteAsync(int id);
}
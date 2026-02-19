using HR_Management_System.Models;

namespace HR_Management_System.Infrastructure.Repositories;

public interface IRequestRepository
{
  Task AddAsync(Request request);
  Task SaveChangesAsync();
  Task<Request?> GetByIdAsync(int id);
  Task<IEnumerable<Request>> GetAllAsync();
  Task<Request?> UpdateAsync(int id, Request request);
  Task<bool> DeleteAsync(int id);
}

using System.Threading.Tasks;
using HR_Management_System.Models;
using Microsoft.EntityFrameworkCore;
using HR_Management_System.Infrastructure.Repositories;

namespace HR_Management_System.Infrastructure.Repositories;

public class SystemUserRepository : ISystemUserRepository
{
  private readonly RhContext _context;

  public SystemUserRepository(RhContext context)
  {
    _context = context;
  }

  public async Task<bool> LoginExists(string login)
  {
    if (string.IsNullOrWhiteSpace(login)) return false;
    return await _context.Systemusers.AnyAsync(u => u.Login == login);
  }

  public async Task AddAsync(Systemuser user)
  {
    await _context.Systemusers.AddAsync(user);
  }

  public async Task SaveChangesAsync()
  {
    await _context.SaveChangesAsync();
  }

  public async Task<Systemuser?> GetByIdAsync(int id)
  {
    return await _context.Systemusers.FindAsync(id);
  }

  public async Task<IEnumerable<Systemuser>> GetAllAsync()
  {
    return await _context.Systemusers.ToListAsync();
  }

  public async Task<Systemuser?> UpdateAsync(int id, Systemuser updatedData)
  {
    var user = await _context.Systemusers.FindAsync(id);

    if (user is null) return null;

    if (updatedData.Login is not null) user.Login = updatedData.Login;
    if (updatedData.Password is not null) user.Password = updatedData.Password;
    if (updatedData.Role is not null) user.Role = updatedData.Role;

    return user;
  }

  public async Task<bool> DeleteAsync(int id)
  {
    var user = await _context.Systemusers.FindAsync(id);

    if (user is null) return false;

    _context.Systemusers.Remove(user);
    await _context.SaveChangesAsync();

    return true;
  }
}
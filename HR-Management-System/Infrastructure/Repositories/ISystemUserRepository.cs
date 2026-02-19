using System.Threading.Tasks;
using HR_Management_System.Models;

namespace HR_Management_System.Infrastructure.Repositories;

public interface ISystemUserRepository
{
  /// <summary>
  /// Checks whether a login already exists in the data store.
  /// </summary>
  Task<bool> LoginExists(string login);

  /// <summary>
  /// Adds a new Systemuser to the data store (does not commit).
  /// </summary>
  Task AddAsync(Systemuser user);

  /// <summary>
  /// Persists pending changes to the data store.
  /// </summary>
  Task SaveChangesAsync();

  /// <summary>
  /// Retrieves a Systemuser by id.
  /// </summary>
  Task<Systemuser?> GetByIdAsync(int id);

  /// <summary>
  /// Retrieves a Systemuser from database.
  /// </summary
  Task<IEnumerable<Systemuser>> GetAllAsync();

  Task<Systemuser?> UpdateAsync(int id, Systemuser user);
}
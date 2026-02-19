using HR_Management_System.Models;
using Microsoft.EntityFrameworkCore;

namespace HR_Management_System.Infrastructure.Repositories;

public class RequestRepository : IRequestRepository
{
  private readonly RhContext _context;

  public RequestRepository(RhContext context)
  {
    _context = context;
  }

  public async Task AddAsync(Request request)
  {
    await _context.Requests.AddAsync(request);
  }

  public async Task SaveChangesAsync()
  {
    await _context.SaveChangesAsync();
  }

  public async Task<Request?> GetByIdAsync(int id)
  {
    return await _context.Requests
        .Include(r => r.Employee)
        .FirstOrDefaultAsync(r => r.Id == id);
  }

  public async Task<IEnumerable<Request>> GetAllAsync()
  {
    return await _context.Requests
        .Include(r => r.Employee)
        .ToListAsync();
  }

  public async Task<Request?> UpdateAsync(int id, Request updatedData)
  {
    var request = await _context.Requests.FindAsync(id);

    if (request is null) return null;

    if (updatedData.StartDate is not null) request.StartDate = updatedData.StartDate;
    if (updatedData.EndDate is not null) request.EndDate = updatedData.EndDate;
    if (updatedData.Status is not null) request.Status = updatedData.Status;
    if (updatedData.Type is not null) request.Type = updatedData.Type;

    return request;
  }

  public async Task<bool> DeleteAsync(int id)
  {
    var request = await _context.Requests.FindAsync(id);

    if (request is null) return false;

    _context.Requests.Remove(request);
    await _context.SaveChangesAsync();

    return true;
  }
}
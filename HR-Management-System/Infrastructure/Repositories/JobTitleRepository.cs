
using HR_Management_System.Models;
using Microsoft.EntityFrameworkCore;

namespace HR_Management_System.Infrastructure.Repositories;

public class JobTitleRepository : IJobTitleRepository
{
  private readonly RhContext _context;

  public JobTitleRepository(RhContext context)
  {
    _context = context;
  }

  public async Task AddAsync(Jobtitle jobTitle)
  {
    await _context.Jobtitles.AddAsync(jobTitle);
  }

  public async Task SaveChangesAsync()
  {
    await _context.SaveChangesAsync();
  }

  public async Task<Jobtitle?> GetByIdAsync(int id)
  {
    return await _context.Jobtitles.FindAsync(id);
  }

  public async Task<IEnumerable<Jobtitle>> GetAllAsync()
  {
    return await _context.Jobtitles.ToListAsync();
  }

  public async Task<Jobtitle?> UpdateAsync(int id, Jobtitle updatedData)
  {
    var jobTitle = await _context.Jobtitles.FindAsync(id);

    if (jobTitle is null) return null;

    if (updatedData.Title is not null) jobTitle.Title = updatedData.Title;
    if (updatedData.Description is not null) jobTitle.Description = updatedData.Description;

    return jobTitle;
  }

  public async Task<bool> DeleteAsync(int id)
  {
    var jobTitle = await _context.Jobtitles.FindAsync(id);

    if (jobTitle is null) return false;

    _context.Jobtitles.Remove(jobTitle);
    await _context.SaveChangesAsync();

    return true;
  }
}
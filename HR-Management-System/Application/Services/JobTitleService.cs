using HR_Management_System.Models;
using HR_Management_System.Infrastructure.Repositories;
using HR_Management_System.Application.Dtos;

namespace HR_Management_System.Application.Services;

public class JobTitleService : IJobTitleService
{
  private readonly IJobTitleRepository _repository;

  public JobTitleService(IJobTitleRepository repository)
  {
    _repository = repository;
  }

  public async Task<JobTitleResponseDto> CreateAsync(JobTitleRequestDto dto)
  {
    if (dto is null) throw new ArgumentNullException(nameof(dto));

    var jobTitle = new Jobtitle
    {
      Title = dto.Title,
      Description = dto.Description
    };

    await _repository.AddAsync(jobTitle);
    await _repository.SaveChangesAsync();

    return MapToResponseDto(jobTitle);
  }

  public async Task<JobTitleResponseDto?> GetByIdAsync(int id)
  {
    var jobTitle = await _repository.GetByIdAsync(id);
    return jobTitle is null ? null : MapToResponseDto(jobTitle);
  }

  public async Task<IEnumerable<JobTitleResponseDto>> GetAllAsync()
  {
    var jobTitles = await _repository.GetAllAsync();
    return jobTitles.Select(MapToResponseDto);
  }

  public async Task<JobTitleResponseDto?> UpdateAsync(int id, JobTitleRequestDto dto)
  {
    if (dto is null) throw new ArgumentNullException(nameof(dto));

    var updatedData = new Jobtitle
    {
      Title = dto.Title,
      Description = dto.Description
    };

    var jobTitle = await _repository.UpdateAsync(id, updatedData);

    if (jobTitle is null) return null;

    await _repository.SaveChangesAsync();

    return MapToResponseDto(jobTitle);
  }

  public async Task<bool> DeleteAsync(int id)
  {
    return await _repository.DeleteAsync(id);
  }

  private static JobTitleResponseDto MapToResponseDto(Jobtitle jobTitle)
  {
    return new JobTitleResponseDto
    {
      Id = jobTitle.Id,
      Title = jobTitle.Title,
      Description = jobTitle.Description
    };
  }
}
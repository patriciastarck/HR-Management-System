
using HR_Management_System.Application.Dtos;

namespace HR_Management_System.Application.Services;

public interface IJobTitleService
{
  Task<JobTitleResponseDto> CreateAsync(JobTitleRequestDto dto);
  Task<JobTitleResponseDto?> GetByIdAsync(int id);
  Task<IEnumerable<JobTitleResponseDto>> GetAllAsync();
  Task<JobTitleResponseDto?> UpdateAsync(int id, JobTitleRequestDto dto);
  Task<bool> DeleteAsync(int id);
}
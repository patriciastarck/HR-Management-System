using HR_Management_System.Application.Dtos;

namespace HR_Management_System.Application.Services;

public interface IRequestService
{
  Task<RequestResponseDto> CreateAsync(RequestDto dto);
  Task<RequestResponseDto?> GetByIdAsync(int id);
  Task<IEnumerable<RequestResponseDto>> GetAllAsync();
  Task<RequestResponseDto?> UpdateAsync(int id, RequestDto dto);
  Task<bool> DeleteAsync(int id);
}

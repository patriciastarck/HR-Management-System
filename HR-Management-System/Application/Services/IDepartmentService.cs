using HR_Management_System.Application.Dtos;

namespace HR_Management_System.Application.Services;

public interface IDepartmentService
{
  Task<DepartmentResponseDto> CreateAsync(DepartmentRequestDto dto);
  Task<DepartmentResponseDto?> GetByIdAsync(int id);
  Task<IEnumerable<DepartmentResponseDto>> GetAllAsync();
  Task<DepartmentResponseDto?> UpdateAsync(int id, DepartmentRequestDto dto);
  Task<bool> DeleteAsync(int id);
}
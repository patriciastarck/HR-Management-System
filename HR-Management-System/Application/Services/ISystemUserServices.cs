using HR_Management_System.Application.Dtos;

namespace HR_Management_System.Application.Services;

public interface ISystemUserService
{
  Task<UserResponseDto> CreateAsync(UserRequestDto dto);
  Task<UserResponseDto?> GetByIdAsync(int id);
  Task<IEnumerable<UserResponseDto>> GetAllAsync();
  Task<UserResponseDto?> UpdateAsync(int id, UserRequestDto dto);
  Task<bool> DeleteAsync(int id); // ← NOVO
}
using HR_Management_System.Application.Dtos;

namespace HR_Management_System.Application.Services;

public interface ISystemUserService
{
  Task<UserResponseDto> CreateAsync(UserRequestDto dto);
  Task<UserResponseDto?> GetByIdAsync(int id);

  // pega a lista de entidades e transformamos em uma lista de dtos
  Task<IEnumerable<UserResponseDto>> GetAllAsync();

  Task<bool> DeleteAsync(int id);
}

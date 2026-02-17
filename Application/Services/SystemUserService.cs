using HR_Management_System.Application.Dtos;
using HR_Management_System.Application;
using HR_Management_System.Repositories;
using HR_Management_System.Models;
using System.Linq;


namespace HR_Management_System.Application.Services;

public class SystemUserService : ISystemUserService
{
  private readonly ISystemUserRepository _repository;

  public SystemUserService(ISystemUserRepository repository)
  {
    _repository = repository;
  }

  public async Task<UserResponseDto> CreateAsync(UserRequestDto dto)
  {
    if (dto is null)
      throw new ArgumentNullException(nameof(dto));

    var user = new Systemuser
    {
      Login = dto.Login,
      Password = dto.Password,
      Role = dto.Role ?? string.Empty
    };

    await _repository.AddAsync(user);
    await _repository.SaveChangesAsync();

    return MapToResponseDto(user);
  }

  public async Task<UserResponseDto?> GetByIdAsync(int id)
  {
    var user = await _repository.GetByIdAsync(id);
    if (user is null) return null;
    return MapToResponseDto(user);
  }

  public async Task<IEnumerable<UserResponseDto>> GetAllAsync()
  {
    var users = await _repository.GetAllAsync();
    return users.Select(MapToResponseDto);
  }

  public async Task<bool> DeleteAsync(int id)
  {
    var user = await _repository.GetByIdAsync(id);
    if (user is null) return false;

    await _repository.DeleteAsync(user);
    await _repository.SaveChangesAsync();
    return true;
  }

  private UserResponseDto MapToResponseDto(Systemuser user)
  {
    return new UserResponseDto
    {
      Id = user.Id,
      Login = user.Login,
      Role = user.Role ?? string.Empty
    };
  }
}

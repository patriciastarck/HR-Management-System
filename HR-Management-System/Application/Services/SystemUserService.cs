using HR_Management_System.Models;
using HR_Management_System.Infrastructure.Repositories;
using HR_Management_System.Application.Dtos;
using FluentValidation;

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

        await ValidateLogin(dto.Login);

        var user = new Systemuser
        {
            Login = dto.Login,
            Password = dto.Password,
            Role = dto.Role
        };

    await _repository.AddAsync(user);
    await _repository.SaveChangesAsync();

    return MapToResponseDto(user);
  }

    public async Task<UserResponseDto?> GetByIdAsync(int id)
    {
        var user = await _repository.GetByIdAsync(id);

        if (user is null)
            return null;

        return MapToResponseDto(user);
    }

    private async Task ValidateLogin(string login)
    {
        if (string.IsNullOrWhiteSpace(login))
            throw new ArgumentException("Login obrigatório.");

        if (login.Length < 4)
            throw new ArgumentException("Login deve ter no mínimo 4 caracteres.");

        if (login.Contains(' '))
            throw new ArgumentException("Login não pode conter espaços.");

        if (await _repository.LoginExists(login))
            throw new InvalidOperationException("Login já existe.");
    }

    private static UserResponseDto MapToResponseDto(Systemuser user)
    {
        return new UserResponseDto
        {
            Id = user.Id,
            Login = user.Login,
            Role = user.Role
        };
    }

    public async Task<IEnumerable<UserResponseDto>> GetAllAsync()
    {
        // Obtemos a lista de entidades do repositório e guardamos na variável users
        var users = await _repository.GetAllAsync();

        // Converte cada systemuser em um UserResponseDto
        // O método Select é parte do LINQ (.Select) serve para mapear a lista de entidades.
        return users.Select(MapToResponseDto);
    }
}

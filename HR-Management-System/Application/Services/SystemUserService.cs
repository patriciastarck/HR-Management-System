using HR_Management_System.Models;
using HR_Management_System.Repositories;
using HR_Management_System.Application.Dtos;
using FluentValidation;

namespace HR_Management_System.Application.Services;

public class SystemUserService : ISystemUserService
{
    private readonly ISystemUserRepository _repository;
    private readonly IValidator<UserRequestDto> _validator;

    public SystemUserService(ISystemUserRepository repository, IValidator<UserRequestDto> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<UserResponseDto> CreateAsync(UserRequestDto dto)
    {
        if (dto is null)
            throw new ArgumentNullException(nameof(dto));

        var validationResult = await _validator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            var errors = string.Join(" ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new ArgumentException(errors);
        }

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
        return user is null ? null : MapToResponseDto(user);
    }

    public async Task<IEnumerable<UserResponseDto>> GetAllAsync()
    {
        var users = await _repository.GetAllAsync();
        return users.Select(MapToResponseDto);
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
}
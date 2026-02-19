using HR_Management_System.Models;
using HR_Management_System.Infrastructure.Repositories;
using HR_Management_System.Application.Dtos;

namespace HR_Management_System.Application.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _repository;

    public EmployeeService(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    public async Task<EmployeeResponseDto> CreateAsync(EmployeeRequestDto dto)
    {
        if (dto is null) throw new ArgumentNullException(nameof(dto));

        var employee = new Employee
        {
            Name = dto.Name,
            Cpf = dto.Cpf,
            // Os campos abaixo serão salvos como NULL no banco ou valores padrão
            IsActive = true,
            HireDate = DateOnly.FromDateTime(DateTime.Now),
            Email = null,
            Salary = null,
            DepartmentId = null,
            JobTitleId = null,
            UserId = null
        };

        await _repository.AddAsync(employee);
        await _repository.SaveChangesAsync();

        return MapToResponseDto(employee);
    }

    public async Task<EmployeeResponseDto?> GetByIdAsync(int id)
    {
        var employee = await _repository.GetByIdAsync(id);
        return employee == null ? null : MapToResponseDto(employee);
    }

    public async Task<IEnumerable<EmployeeResponseDto>> GetAllAsync()
    {
        var employees = await _repository.GetAllAsync();
        return employees.Select(MapToResponseDto);
    }

  public async Task<EmployeeResponseDto?> UpdateAsync(int id, EmployeeUpdateRequestDto dto)
  {
    if (dto is null) throw new ArgumentNullException(nameof(dto));

    var updatedData = new Employee
    {
      Name = dto.Name,
      Email = dto.Email,
      Salary = dto.Salary,
      IsActive = dto.IsActive,
      DepartmentId = dto.DepartmentId,
      JobTitleId = dto.JobTitleId
    };

    var employee = await _repository.UpdateAsync(id, updatedData);

    if (employee is null) return null;

    await _repository.SaveChangesAsync();

    return MapToResponseDto(employee);
  }

  private static EmployeeResponseDto MapToResponseDto(Employee employee)
  {
    return new EmployeeResponseDto
    {
      Id = employee.Id,
      Name = employee.Name,
      Cpf = employee.Cpf,
    };
  }

  public async Task<bool> DeleteAsync(int id)
  {
    return await _repository.DeleteAsync(id);
  }
}
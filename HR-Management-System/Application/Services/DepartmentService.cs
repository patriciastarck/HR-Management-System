using HR_Management_System.Models;
using HR_Management_System.Infrastructure.Repositories;
using HR_Management_System.Application.Dtos;

namespace HR_Management_System.Application.Services;

public class DepartmentService : IDepartmentService
{
  private readonly IDepartmentRepository _repository;

  public DepartmentService(IDepartmentRepository repository)
  {
    _repository = repository;
  }

  public async Task<DepartmentResponseDto> CreateAsync(DepartmentRequestDto dto)
  {
    if (dto is null) throw new ArgumentNullException(nameof(dto));

    var department = new Department { Name = dto.Name };

    await _repository.AddAsync(department);
    await _repository.SaveChangesAsync();

    return MapToResponseDto(department);
  }

  public async Task<DepartmentResponseDto?> GetByIdAsync(int id)
  {
    var department = await _repository.GetByIdAsync(id);
    return department is null ? null : MapToResponseDto(department);
  }

  public async Task<IEnumerable<DepartmentResponseDto>> GetAllAsync()
  {
    var departments = await _repository.GetAllAsync();
    return departments.Select(MapToResponseDto);
  }

  public async Task<DepartmentResponseDto?> UpdateAsync(int id, DepartmentRequestDto dto)
  {
    if (dto is null) throw new ArgumentNullException(nameof(dto));

    var updatedData = new Department { Name = dto.Name };

    var department = await _repository.UpdateAsync(id, updatedData);

    if (department is null) return null;

    await _repository.SaveChangesAsync();

    return MapToResponseDto(department);
  }

  public async Task<bool> DeleteAsync(int id)
  {
    return await _repository.DeleteAsync(id);
  }

  private static DepartmentResponseDto MapToResponseDto(Department department)
  {
    return new DepartmentResponseDto
    {
      Id = department.Id,
      Name = department.Name
    };
  }
}
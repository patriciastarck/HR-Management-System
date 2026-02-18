using HR_Management_System.Application.Dtos;

namespace HR_Management_System.Application.Services;

public interface IEmployeeService
{
    Task<EmployeeResponseDto> CreateAsync(EmployeeRequestDto dto);
    Task<EmployeeResponseDto?> GetByIdAsync(int id);
    Task<IEnumerable<EmployeeResponseDto>> GetAllAsync();
}
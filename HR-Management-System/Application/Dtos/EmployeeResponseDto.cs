namespace HR_Management_System.Application.Dtos;

public class EmployeeResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Cpf { get; set; } = null!;
    //public string? Email { get; set; }
    //public bool IsActive { get; set; }
    //public string? DepartmentName { get; set; }
}
namespace HR_Management_System.Application.Dtos;

public class EmployeeUpdateRequestDto
{
  public string? Name { get; set; }
  public string? Email { get; set; }
  public decimal? Salary { get; set; }
  public bool? IsActive { get; set; }
  public int? DepartmentId { get; set; }
  public int? JobTitleId { get; set; }
}
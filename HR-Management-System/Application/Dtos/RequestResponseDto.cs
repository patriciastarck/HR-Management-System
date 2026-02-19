namespace HR_Management_System.Application.Dtos;

public class RequestResponseDto
{
    public int Id { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string? Status { get; set; }
    public string? Type { get; set; }
    public int EmployeeId { get; set; }
}
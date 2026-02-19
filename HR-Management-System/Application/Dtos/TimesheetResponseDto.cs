namespace HR_Management_System.Application.Dtos;

public class TimesheetResponseDto
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly? EntryTime { get; set; }
    public TimeOnly? ExitTime { get; set; }
    public int EmployeeId { get; set; }

    // O campo que você deseja!
    public string? EmployeeName { get; set; }
}
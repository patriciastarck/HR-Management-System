namespace HR_Management_System.Application.Dtos;

public class TimesheetRequestDto
{
    public int EmployeeId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly? EntryTime { get; set; }
    public TimeOnly? ExitTime { get; set; }
}
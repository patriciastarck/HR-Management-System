namespace HR_Management_System.Models;

public class Timesheet
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly? EntryTime { get; set; }
    public TimeOnly? ExitTime { get; set; }

    // Propriedade calculada para o total de horas
    public TimeSpan? TotalHours => (ExitTime.HasValue && EntryTime.HasValue)
        ? ExitTime.Value - EntryTime.Value
        : null;

    public virtual Employee? Employee { get; set; }
}
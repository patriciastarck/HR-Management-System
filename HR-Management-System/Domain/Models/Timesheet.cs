namespace HR_Management_System.Models; // Ajuste o namespace se necessário

public class Timesheet
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly? EntryTime { get; set; } // Verifique se este nome está EXATAMENTE assim
    public TimeOnly? ExitTime { get; set; }  // Verifique se este nome está EXATAMENTE assim

    // Provavelmente o erro está aqui embaixo, em alguma lógica de cálculo:
    public TimeSpan? TotalHours => (ExitTime.HasValue && EntryTime.HasValue)
        ? ExitTime.Value - EntryTime.Value
        : null;

    public virtual Employee? Employee { get; set; }
}
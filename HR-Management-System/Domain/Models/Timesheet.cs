using System;
using System.Collections.Generic;

namespace HR_Management_System.Models;

public partial class Timesheet
{
    public int Id { get; set; }

    public DateOnly Date { get; set; }

    public TimeOnly? EntryTime { get; set; }

    public TimeOnly? ExitTime { get; set; }

    public int EmployeeId { get; set; }

    public virtual Employee Employee { get; set; } = null!;
}

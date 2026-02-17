using System;
using System.Collections.Generic;

namespace HR_Management_System.Models;

public partial class Trainingparticipation
{
    public int Id { get; set; }

    public DateOnly? StartDate { get; set; }

    public string? Status { get; set; }

    public DateOnly? EndDate { get; set; }

    public string? Type { get; set; }

    public int EmployeeId { get; set; }

    public int TrainingId { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual Training Training { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace HR_Management_System.Models;

public partial class Training
{
    public int Id { get; set; }

    public string Topic { get; set; } = null!;

    public string? Description { get; set; }

    public int? HoursDuration { get; set; }

    public virtual ICollection<Trainingparticipation> Trainingparticipations { get; set; } = new List<Trainingparticipation>();
}

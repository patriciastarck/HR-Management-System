using System;
using System.Collections.Generic;

namespace HR_Management_System.Models;

public partial class Jobtitle
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}

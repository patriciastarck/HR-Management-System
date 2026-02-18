using System;
using System.Collections.Generic;

namespace HR_Management_System.Models;

public partial class Employee
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Cpf { get; set; } = null!;

    public string? Email { get; set; }

    public decimal? Salary { get; set; }

    public bool? IsActive { get; set; }

    public DateOnly? HireDate { get; set; }

    public int? UserId { get; set; }

    public int? DepartmentId { get; set; }

    public int? JobTitleId { get; set; }

    public virtual Department? Department { get; set; }

    public virtual Jobtitle? JobTitle { get; set; }

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();

    public virtual ICollection<Timesheet> Timesheets { get; set; } = new List<Timesheet>();

    public virtual ICollection<Trainingparticipation> Trainingparticipations { get; set; } = new List<Trainingparticipation>();

    public virtual Systemuser? User { get; set; }
}

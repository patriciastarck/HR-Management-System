using System;
using System.Collections.Generic;

namespace HR_Management_System.Models;

public partial class Systemuser
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Role { get; set; }

    public virtual Employee? Employee { get; set; }
}

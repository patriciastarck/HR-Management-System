using System;
using System.Collections.Generic;

namespace HR_Management_System.Models;

public partial class TbDepartamento
{
    public Guid Id { get; set; }

    public string Nome { get; set; } = null!;

    public virtual ICollection<TbFuncionario> TbFuncionarios { get; set; } = new List<TbFuncionario>();
}

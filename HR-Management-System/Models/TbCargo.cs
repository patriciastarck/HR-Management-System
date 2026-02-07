using System;
using System.Collections.Generic;

namespace HR_Management_System.Models;

public partial class TbCargo
{
    public Guid Id { get; set; }

    public string Titulo { get; set; } = null!;

    public string? Descricao { get; set; }

    public virtual ICollection<TbFuncionario> TbFuncionarios { get; set; } = new List<TbFuncionario>();
}

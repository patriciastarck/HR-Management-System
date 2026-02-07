using System;
using System.Collections.Generic;

namespace HR_Management_System.Models;

public partial class TbUsuario
{
    public Guid Id { get; set; }

    public string Login { get; set; } = null!;

    public string Senha { get; set; } = null!;

    public string Papel { get; set; } = null!;

    public virtual ICollection<TbFuncionario> TbFuncionarios { get; set; } = new List<TbFuncionario>();
}

using System;
using System.Collections.Generic;

namespace HR_Management_System.Models;

public partial class TbPonto
{
    public Guid Id { get; set; }

    public DateOnly Data { get; set; }

    public TimeOnly HoraEntrada { get; set; }

    public TimeOnly? HoraSaida { get; set; }

    public Guid FuncionarioId { get; set; }

    public virtual TbFuncionario Funcionario { get; set; } = null!;
}

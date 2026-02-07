using System;
using System.Collections.Generic;

namespace HR_Management_System.Models;

public partial class TbParticipacaoTreinamento
{
    public Guid Id { get; set; }

    public DateOnly DataInicio { get; set; }

    public DateOnly? DataFim { get; set; }

    public string Status { get; set; } = null!;

    public string? Tipo { get; set; }

    public Guid FuncionarioId { get; set; }

    public Guid TreinamentoId { get; set; }

    public virtual TbFuncionario Funcionario { get; set; } = null!;

    public virtual TbTreinamento Treinamento { get; set; } = null!;
}

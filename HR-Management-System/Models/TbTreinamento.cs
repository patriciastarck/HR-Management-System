using System;
using System.Collections.Generic;

namespace HR_Management_System.Models;

public partial class TbTreinamento
{
    public Guid Id { get; set; }

    public string Tema { get; set; } = null!;

    public string? Descricao { get; set; }

    public int CargaHoraria { get; set; }

    public virtual ICollection<TbParticipacaoTreinamento> TbParticipacaoTreinamentos { get; set; } = new List<TbParticipacaoTreinamento>();
}

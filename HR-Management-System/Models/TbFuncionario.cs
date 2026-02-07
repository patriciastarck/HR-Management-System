using System;
using System.Collections.Generic;

namespace HR_Management_System.Models;

public partial class TbFuncionario
{
    public Guid Id { get; set; }

    public string Nome { get; set; } = null!;

    public string Cpf { get; set; } = null!;

    public string Email { get; set; } = null!;

    public decimal Salario { get; set; }

    public bool? Ativo { get; set; }

    public DateOnly DataAdmissao { get; set; }

    public Guid? UsuarioId { get; set; }

    public Guid? DepartamentoId { get; set; }

    public Guid? CargoId { get; set; }

    public virtual TbCargo? Cargo { get; set; }

    public virtual TbDepartamento? Departamento { get; set; }

    public virtual ICollection<TbParticipacaoTreinamento> TbParticipacaoTreinamentos { get; set; } = new List<TbParticipacaoTreinamento>();

    public virtual ICollection<TbPonto> TbPontos { get; set; } = new List<TbPonto>();

    public virtual ICollection<TbSolicitacao> TbSolicitacaos { get; set; } = new List<TbSolicitacao>();

    public virtual TbUsuario? Usuario { get; set; }
}

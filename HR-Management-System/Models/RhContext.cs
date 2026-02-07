using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HR_Management_System.Models;

public partial class RhContext : DbContext
{
    public RhContext()
    {
    }

    public RhContext(DbContextOptions<RhContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TbCargo> TbCargos { get; set; }

    public virtual DbSet<TbDepartamento> TbDepartamentos { get; set; }

    public virtual DbSet<TbFuncionario> TbFuncionarios { get; set; }

    public virtual DbSet<TbParticipacaoTreinamento> TbParticipacaoTreinamentos { get; set; }

    public virtual DbSet<TbPonto> TbPontos { get; set; }

    public virtual DbSet<TbSolicitacao> TbSolicitacaos { get; set; }

    public virtual DbSet<TbTreinamento> TbTreinamentos { get; set; }

    public virtual DbSet<TbUsuario> TbUsuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost:5432;Database=rh;Username=postgres;Password=123456");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TbCargo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tb_cargo_pkey");

            entity.ToTable("tb_cargo");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Descricao).HasColumnName("descricao");
            entity.Property(e => e.Titulo)
                .HasMaxLength(100)
                .HasColumnName("titulo");
        });

        modelBuilder.Entity<TbDepartamento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tb_departamento_pkey");

            entity.ToTable("tb_departamento");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .HasColumnName("nome");
        });

        modelBuilder.Entity<TbFuncionario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tb_funcionario_pkey");

            entity.ToTable("tb_funcionario");

            entity.HasIndex(e => e.Cpf, "tb_funcionario_cpf_key").IsUnique();

            entity.HasIndex(e => e.Email, "tb_funcionario_email_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Ativo)
                .HasDefaultValue(true)
                .HasColumnName("ativo");
            entity.Property(e => e.CargoId).HasColumnName("cargo_id");
            entity.Property(e => e.Cpf)
                .HasMaxLength(11)
                .IsFixedLength()
                .HasColumnName("cpf");
            entity.Property(e => e.DataAdmissao).HasColumnName("data_admissao");
            entity.Property(e => e.DepartamentoId).HasColumnName("departamento_id");
            entity.Property(e => e.Email)
                .HasMaxLength(256)
                .HasColumnName("email");
            entity.Property(e => e.Nome)
                .HasMaxLength(256)
                .HasColumnName("nome");
            entity.Property(e => e.Salario)
                .HasPrecision(10, 2)
                .HasColumnName("salario");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

            entity.HasOne(d => d.Cargo).WithMany(p => p.TbFuncionarios)
                .HasForeignKey(d => d.CargoId)
                .HasConstraintName("fk_func_cargo");

            entity.HasOne(d => d.Departamento).WithMany(p => p.TbFuncionarios)
                .HasForeignKey(d => d.DepartamentoId)
                .HasConstraintName("fk_func_depto");

            entity.HasOne(d => d.Usuario).WithMany(p => p.TbFuncionarios)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("fk_func_usuario");
        });

        modelBuilder.Entity<TbParticipacaoTreinamento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tb_participacao_treinamento_pkey");

            entity.ToTable("tb_participacao_treinamento");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.DataFim).HasColumnName("data_fim");
            entity.Property(e => e.DataInicio).HasColumnName("data_inicio");
            entity.Property(e => e.FuncionarioId).HasColumnName("funcionario_id");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.Tipo)
                .HasMaxLength(50)
                .HasColumnName("tipo");
            entity.Property(e => e.TreinamentoId).HasColumnName("treinamento_id");

            entity.HasOne(d => d.Funcionario).WithMany(p => p.TbParticipacaoTreinamentos)
                .HasForeignKey(d => d.FuncionarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_part_func");

            entity.HasOne(d => d.Treinamento).WithMany(p => p.TbParticipacaoTreinamentos)
                .HasForeignKey(d => d.TreinamentoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_part_treinamento");
        });

        modelBuilder.Entity<TbPonto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tb_ponto_pkey");

            entity.ToTable("tb_ponto");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Data).HasColumnName("data");
            entity.Property(e => e.FuncionarioId).HasColumnName("funcionario_id");
            entity.Property(e => e.HoraEntrada).HasColumnName("hora_entrada");
            entity.Property(e => e.HoraSaida).HasColumnName("hora_saida");

            entity.HasOne(d => d.Funcionario).WithMany(p => p.TbPontos)
                .HasForeignKey(d => d.FuncionarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ponto_func");
        });

        modelBuilder.Entity<TbSolicitacao>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tb_solicitacao_pkey");

            entity.ToTable("tb_solicitacao");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.DataFim).HasColumnName("data_fim");
            entity.Property(e => e.DataInicio).HasColumnName("data_inicio");
            entity.Property(e => e.FuncionarioId).HasColumnName("funcionario_id");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.Tipo)
                .HasMaxLength(100)
                .HasColumnName("tipo");

            entity.HasOne(d => d.Funcionario).WithMany(p => p.TbSolicitacaos)
                .HasForeignKey(d => d.FuncionarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_solic_func");
        });

        modelBuilder.Entity<TbTreinamento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tb_treinamento_pkey");

            entity.ToTable("tb_treinamento");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.CargaHoraria).HasColumnName("carga_horaria");
            entity.Property(e => e.Descricao).HasColumnName("descricao");
            entity.Property(e => e.Tema)
                .HasMaxLength(200)
                .HasColumnName("tema");
        });

        modelBuilder.Entity<TbUsuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tb_usuario_pkey");

            entity.ToTable("tb_usuario");

            entity.HasIndex(e => e.Login, "tb_usuario_login_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .HasColumnName("login");
            entity.Property(e => e.Papel)
                .HasMaxLength(50)
                .HasColumnName("papel");
            entity.Property(e => e.Senha)
                .HasMaxLength(256)
                .HasColumnName("senha");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

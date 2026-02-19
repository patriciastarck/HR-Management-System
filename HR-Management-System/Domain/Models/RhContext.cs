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

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Jobtitle> Jobtitles { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<Systemuser> Systemusers { get; set; }

    public virtual DbSet<Timesheet> Timesheets { get; set; }

    public virtual DbSet<Training> Training { get; set; }

    public virtual DbSet<Trainingparticipation> Trainingparticipations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost:5432;Database=rh;Username=postgres;Password=4435");

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    // Configuration is handled in Program.cs via Dependency Injection
    //    base.OnConfiguring(optionsBuilder);
    //}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("department_pkey");

            entity.ToTable("department");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("employee_pkey");

            entity.ToTable("employee");

            entity.HasIndex(e => e.Cpf, "employee_cpf_key").IsUnique();

            entity.HasIndex(e => e.Email, "employee_email_key").IsUnique();

            entity.HasIndex(e => e.UserId, "employee_user_id_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cpf)
                    .HasMaxLength(14)
                    .HasColumnName("cpf");
            entity.Property(e => e.DepartmentId).HasColumnName("department_id");
            entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .HasColumnName("email");
            entity.Property(e => e.HireDate).HasColumnName("hire_date");
            entity.Property(e => e.IsActive)
                    .HasDefaultValue(true)
                    .HasColumnName("is_active");
            entity.Property(e => e.JobTitleId).HasColumnName("job_title_id");
            entity.Property(e => e.Name)
                    .HasMaxLength(150)
                    .HasColumnName("name");
            entity.Property(e => e.Salary)
                    .HasPrecision(10, 2)
                    .HasColumnName("salary");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Department).WithMany(p => p.Employees)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("fk_department");

            entity.HasOne(d => d.JobTitle).WithMany(p => p.Employees)
                    .HasForeignKey(d => d.JobTitleId)
                    .HasConstraintName("fk_job_title");

            entity.HasOne(d => d.User).WithOne(p => p.Employee)
                    .HasForeignKey<Employee>(d => d.UserId)
                    .HasConstraintName("fk_user");
        });

        modelBuilder.Entity<Jobtitle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("jobtitle_pkey");

            entity.ToTable("jobtitle");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .HasColumnName("title");
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("request_pkey");

            entity.ToTable("request");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");
            entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .HasColumnName("type");

            entity.HasOne(d => d.Employee).WithMany(p => p.Requests)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_employee_request");
        });

        modelBuilder.Entity<Systemuser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("systemuser_pkey");

            entity.ToTable("systemuser");

            entity.HasIndex(e => e.Login, "systemuser_login_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Login)
                    .HasMaxLength(50)
                    .HasColumnName("login");
            entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .HasColumnName("password");
            entity.Property(e => e.Role)
                    .HasMaxLength(50)
                    .HasColumnName("role");
        });

        modelBuilder.Entity<Timesheet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("timesheet_pkey");

            entity.ToTable("timesheet");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.EntryTime).HasColumnName("entry_time");
            entity.Property(e => e.ExitTime).HasColumnName("exit_time");

            entity.HasOne(d => d.Employee).WithMany(p => p.Timesheets)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_employee_timesheet");
        });

        modelBuilder.Entity<Training>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("training_pkey");

            entity.ToTable("training");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.HoursDuration).HasColumnName("hours_duration");
            entity.Property(e => e.Topic)
                    .HasMaxLength(150)
                    .HasColumnName("topic");
        });

        modelBuilder.Entity<Trainingparticipation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("trainingparticipation_pkey");

            entity.ToTable("trainingparticipation");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");

            // Configuração explícita para DateOnly no PostgreSQL
            entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .HasColumnType("date");

            entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .HasColumnType("date");

            entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");

            entity.Property(e => e.TrainingId).HasColumnName("training_id");

            entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .HasColumnName("type");

            // Relacionamento com Employee
            entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Trainingparticipations)
                    .HasForeignKey(d => d.EmployeeId)
                    // Alterado para Cascade para evitar erros de constraint ao gerenciar funcionários
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_employee_participation");

            // Relacionamento com Training
            entity.HasOne(d => d.Training)
                    .WithMany(p => p.Trainingparticipations)
                    .HasForeignKey(d => d.TrainingId)
                    // Alterado para Cascade: se o curso for excluído, as participações saem juntas
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_training_participation");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
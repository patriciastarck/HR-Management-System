using HR_Management_System.Models;
using HR_Management_System.Infrastructure.Repositories;
using HR_Management_System.Application.Services;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.AspNetCore;
using HR_Management_System.Application.Validators;

var builder = WebApplication.CreateBuilder(args);

// --- 1. CONFIGURA��ES PADR�O ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- 2. CONFIGURA��O DO FLUENT VALIDATION ---
// Esta linha � fundamental para que o validador do User e do Employee funcionem!
//builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddValidatorsFromAssemblyContaining<UserRequestDtoValidator>();

// --- 3. REGISTRO DE REPOSIT�RIOS ---
builder.Services.AddScoped<ISystemUserRepository, SystemUserRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>(); // <-- ADICIONADO
builder.Services.AddScoped<ITimesheetRepository, TimesheetRepository>();

// --- 4. REGISTRO DE SERVI�OS ---
builder.Services.AddScoped<ISystemUserService, SystemUserService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>(); // <-- ADICIONADO

builder.Services.AddScoped<IJobTitleRepository, JobTitleRepository>();
builder.Services.AddScoped<IJobTitleService, JobTitleService>();

builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();

builder.Services.AddScoped<IRequestRepository, RequestRepository>();
builder.Services.AddScoped<IRequestService, RequestService>();

// --- 5. BANCO DE DADOS (PostgreSQL) ---
builder.Services.AddDbContext<RhContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// --- 6. PIPELINE DE EXECU��O ---
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
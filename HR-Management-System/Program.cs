using HR_Management_System.Models;
using HR_Management_System.Infrastructure.Repositories;
using HR_Management_System.Application.Services;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.AspNetCore;
using HR_Management_System.Application.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register repository and service for Controller -> Service -> Repository flow
builder.Services.AddScoped<ISystemUserRepository, SystemUserRepository>();
builder.Services.AddScoped<ISystemUserService, SystemUserService>();

builder.Services.AddDbContext<RhContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<UserRequestDtoValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

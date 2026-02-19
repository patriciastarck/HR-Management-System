using Microsoft.AspNetCore.Mvc;
using HR_Management_System.Models;
using HR_Management_System.Infrastructure.Repositories;
using HR_Management_System.Application.Dtos;

namespace HR_Management_System.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TimesheetsController : ControllerBase
{
    private readonly ITimesheetRepository _repository;

    public TimesheetsController(ITimesheetRepository repository)
    {
        _repository = repository;
    }

    [HttpPost]
    public async Task<IActionResult> RecordTime(TimesheetRequestDto dto)
    {
        var timesheet = new Timesheet
        {
            EmployeeId = dto.EmployeeId,
            Date = dto.Date,
            EntryTime = dto.EntryTime,
            ExitTime = dto.ExitTime
        };

        await _repository.AddAsync(timesheet);
        await _repository.SaveChangesAsync();

        return Ok(new { Message = "Ponto registrado com sucesso!" });
    }

    [HttpGet("employee/{employeeId}")]
    public async Task<IActionResult> GetByEmployee(int employeeId)
    {
        // 1. Busca os dados no repositório (que usa o .Include)
        var logs = await _repository.GetByEmployeeIdAsync(employeeId);

        // 2. Mapeia a entidade para o DTO (Isso evita o erro de ciclo JSON)
        var response = logs.Select(t => new TimesheetResponseDto
        {
            Id = t.Id,
            Date = t.Date,
            EntryTime = t.EntryTime,
            ExitTime = t.ExitTime,
            EmployeeId = t.EmployeeId,
            EmployeeName = t.Employee?.Name // Agora o nome será preenchido!
        });

        // 3. Retorna apenas o DTO mapeado
        return Ok(response);
    }
}
using HR_Management_System.Application.Dtos;
using HR_Management_System.Infrastructure.Repositories;
using HR_Management_System.Models;

namespace HR_Management_System.Application.Services;

public class TimesheetService : ITimesheetService
{
    private readonly ITimesheetRepository _repository;

    public TimesheetService(ITimesheetRepository repository)
    {
        _repository = repository;
    }

    public async Task<(bool Success, string Message)> RecordTimeAsync(TimesheetRequestDto dto)
    {
        // Regra: Verificar se já existe registro para este funcionário nesta data
        var existingLogs = await _repository.GetByEmployeeIdAsync(dto.EmployeeId);
        if (existingLogs.Any(t => t.Date == dto.Date))
        {
            return (false, "Já existe um registro de ponto para este funcionário nesta data.");
        }

        var timesheet = new Timesheet
        {
            EmployeeId = dto.EmployeeId,
            Date = dto.Date,
            EntryTime = dto.EntryTime,
            ExitTime = dto.ExitTime
        };

        await _repository.AddAsync(timesheet);
        await _repository.SaveChangesAsync();

        return (true, "Ponto registrado com sucesso!");
    }
}
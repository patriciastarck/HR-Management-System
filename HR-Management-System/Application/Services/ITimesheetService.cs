using HR_Management_System.Application.Dtos;

namespace HR_Management_System.Application.Services;

public interface ITimesheetService
{
    Task<(bool Success, string Message)> RecordTimeAsync(TimesheetRequestDto dto);
}
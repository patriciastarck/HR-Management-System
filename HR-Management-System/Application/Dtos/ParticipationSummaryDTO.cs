namespace HR_Management_System.DTOs;

public record ParticipationSummaryDTO(
    int Id,
    int EmployeeId,
    string? EmployeeName,
    DateOnly? StartDate,
    string? Status
);
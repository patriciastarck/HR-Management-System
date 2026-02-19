namespace HR_Management_System.DTOs;

public record EnrollRequestDto(
    int EmployeeId,
    int TrainingId,
    string? Type = "Internal"
);
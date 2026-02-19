namespace HR_Management_System.DTOs;

public record TrainingResponseDTO(
    int Id,
    string Topic,
    string? Description,
    int? HoursDuration,
    List<ParticipationSummaryDTO> Participations
);
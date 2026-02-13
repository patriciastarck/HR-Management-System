using System.ComponentModel.DataAnnotations;

namespace HR_Management_System.Dtos;

public class UserResponseDto
{
    public int Id { get; set; }
    public string Login { get; set; } = null!;
    public string? Role { get; set; }
}

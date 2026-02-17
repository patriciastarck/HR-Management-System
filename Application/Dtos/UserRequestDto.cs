using System.ComponentModel.DataAnnotations;

namespace HR_Management_System.Application.Dtos; 

public class UserRequestDto
{
    [Required]
    [MaxLength(50)]
    public string Login { get; set; } = null!;

    [Required]
    [MaxLength(255)]
    public string Password { get; set; } = null!;

    [MaxLength(50)]
    public string? Role { get; set; }
}
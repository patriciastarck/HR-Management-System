using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_Management_System.Application.Dtos
{
  public class JobTitleResponseDto
  {
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
  }
}
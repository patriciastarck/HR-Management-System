using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_Management_System.Application.Dtos
{
  public class JobTitleRequestDto
  {
    public string Title { get; set; } = null!;
    public string? Description { get; set; }


  }
}
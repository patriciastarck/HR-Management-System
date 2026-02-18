using Microsoft.AspNetCore.Mvc;
using HR_Management_System.Application.Dtos;
using HR_Management_System.Application.Services;

namespace HR_Management_System.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
  private readonly ISystemUserService _service;

  public UsersController(ISystemUserService service)
  {
    _service = service;
  }

  [HttpPost]
  public async Task<IActionResult> Create(UserRequestDto dto)
  {
    var created = await _service.CreateAsync(dto);

    return CreatedAtAction(nameof(GetById),
        new { id = created.Id },
        created);
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> GetById(int id)
  {
    var user = await _service.GetByIdAsync(id);

    if (user is null)
      return NotFound();

    return Ok(user);
  }

  [HttpGet] 
  public async Task<IActionResult> GetAll()
  {
    // o controller pede a lista de usuarios ao servide
    var users = await _service.GetAllAsync();

        // retorna a lista de usuarios para o cliente com status 200 OK
        return Ok(users);
    }
}

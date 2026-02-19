
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
    return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> GetById(int id)
  {
    var user = await _service.GetByIdAsync(id);
    if (user is null) return NotFound();
    return Ok(user);
  }

  [HttpGet]
  public async Task<IActionResult> GetAll()
  {
    var users = await _service.GetAllAsync();
    return Ok(users);
  }

  [HttpPut("{id}")]
  public async Task<IActionResult> Update(int id, UserRequestDto dto)
  {
    var updated = await _service.UpdateAsync(id, dto);
    if (updated is null) return NotFound();
    return Ok(updated);
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> Delete(int id)
  {
    var deleted = await _service.DeleteAsync(id);
    if (!deleted) return NotFound();
    return NoContent(); // 204
  }
}
using Microsoft.AspNetCore.Mvc;
using HR_Management_System.Application.Dtos;
using HR_Management_System.Application.Services;

namespace HR_Management_System.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentsController : ControllerBase
{
  private readonly IDepartmentService _service;

  public DepartmentsController(IDepartmentService service)
  {
    _service = service;
  }

  [HttpPost]
  public async Task<IActionResult> Create(DepartmentRequestDto dto)
  {
    var created = await _service.CreateAsync(dto);
    return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
  }

  [HttpGet]
  public async Task<IActionResult> GetAll()
  {
    return Ok(await _service.GetAllAsync());
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> GetById(int id)
  {
    var department = await _service.GetByIdAsync(id);
    if (department is null) return NotFound();
    return Ok(department);
  }

  [HttpPut("{id}")]
  public async Task<IActionResult> Update(int id, DepartmentRequestDto dto)
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
    return NoContent();
  }
}


using Microsoft.AspNetCore.Mvc;
using HR_Management_System.Application.Dtos;
using HR_Management_System.Application.Services;

namespace HR_Management_System.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobTitlesController : ControllerBase
{
  private readonly IJobTitleService _service;

  public JobTitlesController(IJobTitleService service)
  {
    _service = service;
  }

  [HttpPost]
  public async Task<IActionResult> Create(JobTitleRequestDto dto)
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
    var jobTitle = await _service.GetByIdAsync(id);
    if (jobTitle is null) return NotFound();
    return Ok(jobTitle);
  }

  [HttpPut("{id}")]
  public async Task<IActionResult> Update(int id, JobTitleRequestDto dto)
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
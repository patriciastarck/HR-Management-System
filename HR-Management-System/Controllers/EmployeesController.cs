using Microsoft.AspNetCore.Mvc;
using HR_Management_System.Application.Dtos;
using HR_Management_System.Application.Services;
using FluentValidation;

namespace HR_Management_System.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
  private readonly IEmployeeService _service;
  private readonly IValidator<EmployeeRequestDto> _validator;

  public EmployeesController(IEmployeeService service, IValidator<EmployeeRequestDto> validator)
  {
    _service = service;
    _validator = validator;
  }

  [HttpPost]
  public async Task<IActionResult> Create(EmployeeRequestDto dto)
  {
    var validationResult = await _validator.ValidateAsync(dto);

    if (!validationResult.IsValid)
    {
      foreach (var error in validationResult.Errors)
      {
        ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
      }
      return ValidationProblem(ModelState);
    }

    var created = await _service.CreateAsync(dto);
    return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> GetById(int id)
  {
    var employee = await _service.GetByIdAsync(id);
    return employee == null ? NotFound() : Ok(employee);
  }

  // ← GetAll substituído com Query Parameters
  [HttpGet]
  public async Task<IActionResult> GetAll(
      [FromQuery] string? name,
      [FromQuery] bool? isActive)
  {
    var employees = await _service.GetAllFilteredAsync(name, isActive);
    return Ok(employees);
  }

  [HttpPut("{id}")]
  public async Task<IActionResult> Update(int id, EmployeeUpdateRequestDto dto)
  {
    var updated = await _service.UpdateAsync(id, dto);

    if (updated is null)
      return NotFound();

    return Ok(updated);
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> Delete(int id)
  {
    var deleted = await _service.DeleteAsync(id);

    if (!deleted)
      return NotFound();

    return NoContent();
  }
}
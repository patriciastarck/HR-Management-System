using Microsoft.AspNetCore.Mvc;
using HR_Management_System.Application.Dtos;
using HR_Management_System.Application.Services;
using FluentValidation; // Adicione este using

namespace HR_Management_System.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
  private readonly IEmployeeService _service;
  private readonly IValidator<EmployeeRequestDto> _validator; // Declarar o validador

  public EmployeesController(IEmployeeService service, IValidator<EmployeeRequestDto> validator)
  {
    _service = service;
    _validator = validator; // Injetar o validador corretamente
  }

  [HttpPost]
  public async Task<IActionResult> Create(EmployeeRequestDto dto)
  {
    // 1. Executa a validação de forma ASSÍNCRONA
    var validationResult = await _validator.ValidateAsync(dto);

    // 2. Verifica se houve erros
    if (!validationResult.IsValid)
    {
      // Adiciona os erros ao ModelState para manter o padrão de resposta da API
      foreach (var error in validationResult.Errors)
      {
        ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
      }
      return ValidationProblem(ModelState);
    }

    // 3. Se estiver tudo OK, segue para o serviço
    var created = await _service.CreateAsync(dto);
    return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> GetById(int id)
  {
    var employee = await _service.GetByIdAsync(id);
    return employee == null ? NotFound() : Ok(employee);
  }

  [HttpGet]
  public async Task<IActionResult> GetAll()
  {
    return Ok(await _service.GetAllAsync());
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

    return NoContent(); // 204 - padrão para delete bem-sucedido
  }
}
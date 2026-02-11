using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HR_Management_System.Models;
using HR_Management_System.Dtos;
using HR_Management_System.Services;

namespace HR_Management_System.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly SystemUserService _service;

    public UsersController(SystemUserService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
    {
        if (dto is null)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = new Systemuser
        {
            Login = dto.Login,
            Password = dto.Password,
            Role = dto.Role
        };

        try
        {
            var created = await _service.CreateAsync(user);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (ArgumentException ex)
        {
            // validation problems -> bad request
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            // uniqueness or repository failure -> conflict
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _service.GetByIdAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        return Ok(user);
    }
}
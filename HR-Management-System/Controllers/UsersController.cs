using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HR_Management_System.Models;
using HR_Management_System.Dtos;

namespace HR_Management_System.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly RhContext _context;

    public UsersController(RhContext context)
    {
        _context = context;
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

        await _context.Systemusers.AddAsync(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _context.Systemusers.FindAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        return Ok(user);
    }
}
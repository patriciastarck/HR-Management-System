using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HR_Management_System.Models;
using HR_Management_System.DTOs;

namespace HR_Management_System.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TrainingController : ControllerBase
{
    private readonly RhContext _context;

    public TrainingController(RhContext context)
    {
        _context = context;
    }

    // GET: api/Training
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TrainingResponseDTO>>> GetTrainings()
    {
        var trainings = await _context.Training
            .Select(t => new TrainingResponseDTO(
                t.Id,
                t.Topic,
                t.Description,
                t.HoursDuration,
                t.Trainingparticipations.Select(p => new ParticipationSummaryDTO(
                    p.Id,
                    p.EmployeeId,
                    p.Employee.Name,
                    p.StartDate,
                    p.Status
                )).ToList()
            ))
            .ToListAsync();

        return Ok(trainings);
    }

    // GET: api/Training/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TrainingResponseDTO>> GetTraining(int id)
    {
        var training = await _context.Training
            .Select(t => new TrainingResponseDTO(
                t.Id,
                t.Topic,
                t.Description,
                t.HoursDuration,
                t.Trainingparticipations.Select(p => new ParticipationSummaryDTO(
                    p.Id,
                    p.EmployeeId,
                    p.Employee.Name,
                    p.StartDate,
                    p.Status
                )).ToList()
            ))
            .FirstOrDefaultAsync(t => t.Id == id);

        if (training == null) return NotFound();

        return Ok(training);
    }

    // POST: api/Training
    [HttpPost]
    public async Task<ActionResult<Training>> PostTraining(Training training)
    {
        _context.Training.Add(training);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTraining), new { id = training.Id }, training);
    }

    // POST: api/Training/Enroll
    [HttpPost("Enroll")]
    public async Task<IActionResult> EnrollEmployee([FromBody] EnrollRequestDto request)
    {
        var trainingExists = await _context.Training.AnyAsync(t => t.Id == request.TrainingId);
        var employeeExists = await _context.Employees.AnyAsync(e => e.Id == request.EmployeeId);

        if (!trainingExists || !employeeExists)
            return BadRequest("Treinamento ou Funcionário não encontrado.");

        var alreadyEnrolled = await _context.Trainingparticipations
            .AnyAsync(p => p.TrainingId == request.TrainingId && p.EmployeeId == request.EmployeeId);

        if (alreadyEnrolled)
            return Conflict("Funcionário já matriculado neste treinamento.");

        var participation = new Trainingparticipation
        {
            TrainingId = request.TrainingId,
            EmployeeId = request.EmployeeId,
            StartDate = DateOnly.FromDateTime(DateTime.Now),
            Status = "Enrolled",
            Type = request.Type ?? "Regular"
        };

        _context.Trainingparticipations.Add(participation);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Matrícula realizada com sucesso!" });
    }

    // DELETE: api/Training/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTraining(int id)
    {
        var training = await _context.Training.FindAsync(id);
        if (training == null) return NotFound();

        _context.Training.Remove(training);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
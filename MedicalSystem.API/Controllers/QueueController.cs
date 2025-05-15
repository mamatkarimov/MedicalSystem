using MedicalSystem.Domain.Entities;
using MedicalSystem.Infrastructure.Data;
using MedicalSystem.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicalSystem.API.Controllers
{
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class QueueController : ControllerBase
{
    private readonly    AppDbContext _context;

    public QueueController(AppDbContext context)
    {
        _context = context;
    }

    [Authorize(Roles = "Admin,Reception,Doctor")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PatientQueue>>> GetQueue()
    {
        return await _context.PatientQueues
            .Include(q => q.Patient)
            .Include(q => q.Appointment)
            .Include(q => q.Department)
            .Where(q => q.Status == "Waiting")
            .OrderBy(q => q.Priority)
            .ThenBy(q => q.QueueDate)
            .ToListAsync();
    }

    [Authorize(Roles = "Admin,Reception")]
    [HttpPost]
    public async Task<ActionResult<PatientQueue>> AddToQueue(PatientQueue queue)
    {
        var patient = await _context.Patients.FindAsync(queue.PatientID);
        if (patient == null || !patient.IsActive)
        {
            return NotFound("Patient not found");
        }

        if (queue.AppointmentID.HasValue)
        {
            var appointment = await _context.Appointments.FindAsync(queue.AppointmentID);
            if (appointment == null)
            {
                return NotFound("Appointment not found");
            }
        }

        queue.QueueDate = DateTime.UtcNow;
        queue.Status = "Waiting";

        _context.PatientQueues.Add(queue);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetQueueItem", new { id = queue.QueueID }, queue);
    }

    [Authorize(Roles = "Admin,Reception,Doctor")]
    [HttpPut("{id}/call")]
    public async Task<IActionResult> CallPatient(int id)
    {
        var queueItem = await _context.PatientQueues.FindAsync(id);
        if (queueItem == null)
        {
            return NotFound();
        }

        queueItem.Status = "InProgress";
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [Authorize(Roles = "Admin,Reception,Doctor")]
    [HttpPut("{id}/complete")]
    public async Task<IActionResult> CompleteQueueItem(int id)
    {
        var queueItem = await _context.PatientQueues.FindAsync(id);
        if (queueItem == null)
        {
            return NotFound();
        }

        queueItem.Status = "Completed";
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [Authorize(Roles = "Admin,Reception")]
    [HttpGet("monitor")]
    public async Task<ActionResult<IEnumerable<PatientQueue>>> GetQueueForMonitor()
    {
        return await _context.PatientQueues
            .Include(q => q.Patient)
            .Include(q => q.Department)
            .Where(q => q.Status == "Waiting" || q.Status == "InProgress")
            .OrderBy(q => q.Status) // InProgress first
            .ThenBy(q => q.Priority)
            .ThenBy(q => q.QueueDate)
            .ToListAsync();
    }
}
}
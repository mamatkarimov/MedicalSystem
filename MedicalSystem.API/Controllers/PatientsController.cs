using MedicalSystem.Application.Models.Requests;
using MedicalSystem.Domain.Entities;
using MedicalSystem.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicalSystem.API.Controllers
{
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class PatientsController : ControllerBase
{
    private readonly AppDbContext _context;    

    public PatientsController(AppDbContext context)
    {
        _context = context;
    }

    [Authorize(Roles = "Admin,Reception")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Patient>>> GetPatients()
    {
        return await _context.Patients.Where(p => p.IsActive).ToListAsync();
    }

    [Authorize(Roles = "Admin,Reception")]
    [HttpGet("{id}")]
    public async Task<ActionResult<Patient>> GetPatient(Guid id)
    {
        var patient = await _context.Patients.FindAsync(id);

        if (patient == null || !patient.IsActive)
        {
            return NotFound();
        }

        return patient;
    }          

    [Authorize(Roles = "Admin,Reception")]
    [HttpPost("register")]
    public async Task<ActionResult<Patient>> RegisterByReception(RegisterPatientRequest request)
    {
        // Same as self-register but without creating user account
        var patient = new Patient
        {
            FirstName = request.FirstName,
            LastName = request.LastName,  
            DateOfBirth = request.BirthDate,
            Gender = request.Gender            
        };

        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetPatient", new { id = patient.Id }, patient);
    }

    [Authorize(Roles = "Admin,Reception")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePatient(Guid id, Patient patient)
    {
        if (id != patient.Id)
        {
            return BadRequest();
        }

        _context.Entry(patient).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PatientExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePatient(Guid id)
    {
        var patient = await _context.Patients.FindAsync(id);
        if (patient == null)
        {
            return NotFound();
        }

        //patient.IsActive = false;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool PatientExists(Guid id)
    {
        return _context.Patients.Any(e => e.Id == id);
    }
}
}
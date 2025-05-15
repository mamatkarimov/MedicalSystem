using MedicalSystem.Application.Models.Requests;
using MedicalSystem.Domain.Entities;
using MedicalSystem.Infrastructure.Data;
using MedicalSystem.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MedicalSystem.API.Controllers
{
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AppointmentsController : ControllerBase
{
    private readonly AppDbContext _context;

    public AppointmentsController(AppDbContext context)
    {
        _context = context;
    }

    [Authorize(Roles = "Admin,Reception,Doctor")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointments()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRoles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

        IQueryable<Appointment> query = _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor);

        // Doctors can see only their appointments
        //if (userRoles.Contains("Doctor"))
        //{
        //    query = query.Where(a => a.DoctorID == userId);
        //}

        return await query.ToListAsync();
    }

    [Authorize(Roles = "Admin,Reception,Doctor")]
    [HttpGet("{id}")]
    public async Task<ActionResult<Appointment>> GetAppointment(int id)
    {
        var appointment = await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .FirstOrDefaultAsync(a => a.AppointmentID == id);

        if (appointment == null)
        {
            return NotFound();
        }

        // Check if doctor is trying to access someone else's appointment
        //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //var isDoctor = User.IsInRole("Doctor");
        //if (isDoctor && appointment.Doctor.Id != userId)
        //{
        //    return Forbid();
        //}

        return appointment;
    }

    [Authorize(Roles = "Admin,Reception,Patient")]
    [HttpPost]
    public async Task<ActionResult<Appointment>> CreateAppointment(CreateAppointmentRequest request)
    {
        // Check if patient exists
        var patient = await _context.Patients.FindAsync(request.PatientID);
        if (patient == null || !patient.IsActive)
        {
            return BadRequest("Patient not found");
        }

        // Check if doctor exists
        var doctor = await _context.Users.FindAsync(request.DoctorID);
        if (doctor == null)
        {
            return BadRequest("Doctor not found");
        }

        // For patients, check if they're creating appointment for themselves
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (User.IsInRole("Patient"))
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null || 
                !user.FirstName.Equals(patient.FirstName, StringComparison.OrdinalIgnoreCase) ||
                !user.LastName.Equals(patient.LastName, StringComparison.OrdinalIgnoreCase))
            {
                return Forbid();
            }
        }

        var appointment = new Appointment
        {
            PatientID = request.PatientID,
            //DoctorID = request.DoctorID,
            AppointmentDate = request.AppointmentDate,
            Status = "Scheduled",
            Reason = request.Reason
        };

        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetAppointment", new { id = appointment.AppointmentID }, appointment);
    }

    [Authorize(Roles = "Admin,Reception,Doctor")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAppointment(int id, Appointment appointment)
    {
        if (id != appointment.AppointmentID)
        {
            return BadRequest();
        }

        // Check permissions
        var existingAppointment = await _context.Appointments.FindAsync(id);
        if (existingAppointment == null)
        {
            return NotFound();
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //if (User.IsInRole("Doctor") && existingAppointment.DoctorID != userId)
        //{
        //    return Forbid();
        //}

        _context.Entry(existingAppointment).CurrentValues.SetValues(appointment);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AppointmentExists(id))
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

    [Authorize(Roles = "Admin,Reception")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> CancelAppointment(int id)
    {
        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment == null)
        {
            return NotFound();
        }

        appointment.Status = "Cancelled";
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool AppointmentExists(int id)
    {
        return _context.Appointments.Any(e => e.AppointmentID == id);
    }
}
}
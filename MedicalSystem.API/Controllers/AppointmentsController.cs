using MedicalSystem.Application.Models.Requests;
using MedicalSystem.Domain.Entities;
using MedicalSystem.Domain.Enums;
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
            if (userRoles.Contains("Doctor") && Guid.TryParse(userId, out var doctorGuid))
            {
                query = query.Where(a => a.DoctorId == doctorGuid);
            }

            return await query.ToListAsync();
        }

        [Authorize(Roles = "Admin,Reception,Doctor")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Appointment>> GetAppointment(Guid id)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
            {
                return NotFound();
            }          

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

            // Check if appointment date is in the future
            if (request.AppointmentDate <= DateTime.UtcNow)
            {
                return BadRequest("Appointment date must be in the future");
            }

            // Check if appointment already exists for the same patient and doctor at the same time
            var existingAppointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.PatientId == request.PatientID &&
                                          a.DoctorId == request.DoctorID &&
                                          a.AppointmentDate == request.AppointmentDate);
            if (existingAppointment != null)
            {
                return Conflict("An appointment already exists for this patient and doctor at the specified time.");
            }
           
            var appointment = new Appointment
            {
                PatientId = request.PatientID,
                DoctorId = request.DoctorID,
                AppointmentDate = request.AppointmentDate,
                Status = "Scheduled",
                Symptoms = request.Symptoms
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAppointment", new { id = appointment.Id }, appointment);
        }

        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> BookAppointment([FromBody] AppointmentRequest request)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var patientId))
                    return Unauthorized("User ID not found or invalid");

                var doctor = await _context.Users
    .Include(u => u.UserRoles)
        .ThenInclude(ur => ur.Role)
    .FirstOrDefaultAsync(u => u.Id == request.DoctorId);

                if (doctor == null || !doctor.UserRoles.Any(r => r.Role.Name == UserRoles.Doctor))
                    return BadRequest("Invalid doctor ID");

                var appointment = new Appointment
                {
                    Id = Guid.NewGuid(),
                    PatientId = patientId,
                    DoctorId = request.DoctorId,
                    AppointmentDate = request.Date,
                    Symptoms = request.Symptoms,
                    Status = "Pending"
                };

                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();

                return Ok(new { appointment.Id });
            }
            catch (Exception ex)
            {
                // Log or return error for debugging
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin,Reception,Doctor")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(Guid id, Appointment appointment)
        {
            if (id != appointment.Id)
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

        private bool AppointmentExists(Guid id)
        {
            return _context.Appointments.Any(e => e.Id == id);
        }


        [HttpGet("mine")]
        public async Task<IActionResult> GetMyAppointments()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var patientId))
                return Unauthorized();

            var appointments = await _context.Appointments
                .Where(a => a.PatientId == patientId)
                .Include(a => a.Doctor)
                .OrderByDescending(a => a.AppointmentDate)
                .Select(a => new
                {
                    a.Id,
                    a.AppointmentDate,
                    a.Symptoms,
                    a.Status,
                    Doctor = a.Doctor.Username
                })
                .ToListAsync();

            return Ok(appointments);
        }

       

        [HttpGet("doctor")]
        [Authorize(Policy = "Doctor")]
        public async Task<IActionResult> GetDoctorAppointments()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var doctorId))
                return Unauthorized();

            var appointments = await _context.Appointments
                .Where(a => a.DoctorId == doctorId)
                .Include(a => a.Patient)
                .OrderBy(a => a.AppointmentDate)
                .Select(a => new
                {
                    a.Id,
                    a.AppointmentDate,
                    a.Symptoms,
                    a.Status,
                    Patient = a.Patient.User.Username
                })
                .ToListAsync();

            return Ok(appointments);
        }
    }
}
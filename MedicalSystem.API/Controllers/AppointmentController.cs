using MedicalSystem.Domain.Entities;
using MedicalSystem.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MedicalSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AppointmentController(AppDbContext context)
        {
            _context = context;
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

                var doctor = await _context.Users.FindAsync(request.DoctorId);
                if (doctor == null || doctor.Role != "Doctor")
                    return BadRequest("Invalid doctor ID");

                var appointment = new Appointment
                {
                    Id = Guid.NewGuid(),
                    PatientId = patientId,
                    DoctorId = request.DoctorId,
                    Date = request.Date,
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

        public class AppointmentRequest
        {
            public Guid DoctorId { get; set; }
            public DateTime Date { get; set; }
            public string Symptoms { get; set; } = string.Empty;
        }
    }
}

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

        // 📌 POST /api/appointment
        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> BookAppointment([FromBody] AppointmentRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var patientId))
                return Unauthorized();

            var doctor = await _context.Users.FindAsync(request.DoctorId);
            if (doctor == null || doctor.Role != "Doctor")
                return BadRequest("Invalid doctor");

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

        public class AppointmentRequest
        {
            public Guid DoctorId { get; set; }
            public DateTime Date { get; set; }
            public string Symptoms { get; set; } = string.Empty;
        }
    }
}

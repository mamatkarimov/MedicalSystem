using MedicalSystem.Infrastructure.Persistence;
using static System.Net.WebRequestMethods;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace MedicalSystem.API.Endpoints
{
    public static class AppointmentEndpoints
    {
        public static void MapAuthEndpoints(this WebApplication app)
        {
            app.MapGet("/api/appointment/doctor", async (HttpContext http, AppDbContext db) =>
{
    var userIdClaim = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var doctorId))
        return Results.Unauthorized();

    var appointments = await db.Appointments
        .Where(a => a.DoctorId == doctorId)
        .Include(a => a.Patient)
        .OrderBy(a => a.Date)
        .Select(a => new
        {
            a.Id,
            a.Date,
            a.Symptoms,
            a.Status,
            Patient = a.Patient.Username
        })
        .ToListAsync();

    return Results.Ok(appointments);
})
.RequireAuthorization("Doctor");
        }
    }
}

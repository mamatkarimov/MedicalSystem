using MedicalSystem.Infrastructure.Persistence;
using static System.Net.WebRequestMethods;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using MedicalSystem.Application.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using MedicalSystem.Domain.Entities;
using MedicalSystem.Domain.Enums;

namespace MedicalSystem.API.Endpoints
{
    public static class AppointmentEndpoints
    {
        static async Task<Guid> GetRoleIdAsync(AppDbContext db, string roleName)
        {
            var role = await db.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            if (role == null) throw new Exception($"Role '{roleName}' not found.");
            return role.Id;
        }

        public static void MapAppointmentEndpoints(this WebApplication app)
        {
            app.MapPost("/api/auth/register-patient", async ([FromServices] AppDbContext db, [FromBody] RegisterPatientRequest request) =>
            {
                if (await db.Users.AnyAsync(u => u.Username == request.Username))
                    return Results.BadRequest("Username already taken");

                var user = new User
                {
                    Username = request.Username,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    UserRoles = new List<UserRole>
        {
            new UserRole { RoleId = await GetRoleIdAsync(db, UserRoles.Patient) }
        }
                };

                db.Users.Add(user);
                await db.SaveChangesAsync();

                db.Patients.Add(new Patient
                {
                    UserId = user.Id,
                    DateOfBirth = request.DateOfBirth,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Gender = request.Gender
                });

                await db.SaveChangesAsync();

                return Results.Ok("Patient registered successfully");
            });

            app.MapPost("/api/auth/register-staff", async ([FromServices] AppDbContext db, [FromBody] RegisterStaffRequest request) =>
            {
                if (await db.Users.AnyAsync(u => u.Username == request.Username))
                    return Results.BadRequest("Username already taken");

                var user = new User
                {
                    Username = request.Username,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    Email = request.Email,
                    UserRoles = new List<UserRole>
                                {
                                    new UserRole { RoleId = await GetRoleIdAsync(db, UserRoles.Doctor) }
                                }
                };

                db.Users.Add(user);
                await db.SaveChangesAsync();

                db.StaffProfiles.Add(new StaffProfile
                {
                    UserId = user.Id,
                    Position = request.Role,
                    Department = request.Department
                });

                await db.SaveChangesAsync();

                return Results.Ok("Doctor registered successfully");
            });

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
            Patient = a.Patient.User.Username
        })
        .ToListAsync();

    return Results.Ok(appointments);
})
.RequireAuthorization("Doctor");
        }
    }
}

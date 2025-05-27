using MedicalSystem.API.Models.User;
using MedicalSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

//namespace MedicalSystem.API.Endpoints
//{
public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        app.MapGet("/api/users", async (AppDbContext db) =>
        {
            var users = await db.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
                .Select(u => new UserListItem
                {
                    Id = u.Id,
                    Username = u.Username,
                    Role = string.Join(", ", u.UserRoles.Select(ur => ur.Role.Name))
                })
                .ToListAsync();

            return Results.Ok(users);
        }).RequireAuthorization(policy => policy.RequireRole("Admin"));

        app.MapPut("/api/users/{id:guid}/role", async (
    Guid id,
    UpdateUserRoleRequest request,
    AppDbContext db) =>
        {
            var user = await db.Users.FindAsync(id);

            if (user is null)
                return Results.NotFound("User not found");

            //user.UserRoles = request.;
            await db.SaveChangesAsync();

            return Results.Ok($"User role updated to {request.Role}");
        }).RequireAuthorization(policy => policy.RequireRole("Admin"));


        app.MapGet("/api/appointment/mine", async (HttpContext http, AppDbContext db) =>
        {
            var userIdClaim = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var patientId))
                return Results.Unauthorized();

            var appointments = await db.Appointments
                .Where(a => a.PatientId == patientId)
                .Include(a => a.Doctor)
                .OrderByDescending(a => a.Date)
                .Select(a => new
                {
                    a.Id,
                    a.Date,
                    a.Symptoms,
                    a.Status,
                    Doctor = a.Doctor.Username
                })
                .ToListAsync();

            return Results.Ok(appointments);
        })
.RequireAuthorization("Patient");
    }


}
//}

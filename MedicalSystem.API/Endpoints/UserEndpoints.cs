using MedicalSystem.API.Models.User;
using MedicalSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

//namespace MedicalSystem.API.Endpoints
//{
public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        app.MapGet("/api/users", async (AppDbContext db) =>
        {
            var users = await db.Users
                .Select(u => new UserListItem
                {
                    Id = u.Id,
                    Username = u.Username,
                    Role = u.Role
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

            user.Role = request.Role;
            await db.SaveChangesAsync();

            return Results.Ok($"User role updated to {request.Role}");
        }).RequireAuthorization(policy => policy.RequireRole("Admin"));

    }


}
//}

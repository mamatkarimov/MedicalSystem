using MedicalSystem.API.Models.Auth;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MedicalSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MedicalSystem.Domain.Entities;
using MedicalSystem.Domain.Enums;

namespace MedicalSystem.API.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        app.MapPost("/api/auth/login", async ([FromServices] AppDbContext db, [FromBody] LoginRequest request) =>
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return Results.Unauthorized();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Username)
        
    };
            foreach (var role in user.UserRoles.Select(ur => ur.Role.Name))
            {
                claims = claims.Append(new Claim(ClaimTypes.Role, role)).ToArray();
            }

            var config = app.Services.GetRequiredService<IConfiguration>();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return Results.Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        });

        app.MapPost("/api/auth/register", async ([FromServices] AppDbContext db, [FromBody] LoginRequest request) =>
        {
            if (await db.Users.AnyAsync(u => u.Username == request.Username))
                return Results.BadRequest("Username already taken");

            var roleName = string.IsNullOrEmpty(request.Role) ? UserRoles.User : request.Role;

            // Find role by name
            var role = await db.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            if (role == null)
                return Results.BadRequest("Invalid role");


            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var user = new User
            {
                Username = request.Username,
                PasswordHash = hashedPassword,
                UserRoles = new List<UserRole>
        {
            new UserRole
            {
                Role = role
            }
        }
            };

            db.Users.Add(user);
            await db.SaveChangesAsync();

            return Results.Ok("User registered");
        });

        app.MapGet("/api/doctor/data", (ClaimsPrincipal user) =>
        {
            var username = user.Identity?.Name;
            return Results.Ok($"Doctor-only data for {username}");
        }).RequireAuthorization(policy => policy.RequireRole(UserRoles.Doctor));

        app.MapGet("/api/admin/data", (ClaimsPrincipal user) =>
        {
            return Results.Ok("This is for Admins only.");
        }).RequireAuthorization(policy => policy.RequireRole(UserRoles.Admin));

        app.MapGet("/api/auth/me", async (ClaimsPrincipal user, AppDbContext db) =>
        {
            var username = user.Identity?.Name;

            if (string.IsNullOrEmpty(username))
                return Results.Unauthorized();

            var found = await db.Users
                .Where(u => u.Username == username)
                .Select(u => new UserInfoResponse
                {
                    Username = u.Username,
                    UserRoles = u.UserRoles
                })
                .FirstOrDefaultAsync();

            return found is not null
                ? Results.Ok(found)
                : Results.NotFound("User not found");
        }).RequireAuthorization();
    }
}

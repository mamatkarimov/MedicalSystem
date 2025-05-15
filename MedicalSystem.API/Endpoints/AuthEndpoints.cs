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
        //    app.MapPost("/api/auth/login", ([FromBody] LoginRequest request) =>
        //    {
        //        // TODO: validate against real DB
        //        if (request.Username == "admin" && request.Password == "password")
        //        {
        //            var claims = new[]
        //            {
        //                new Claim(ClaimTypes.Name, request.Username),
        //                new Claim(ClaimTypes.Role, "Admin")
        //            };

        //            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisIsASuperLongSecretKeyWithAtLeast32Characters"));

        //            var configuration = app.Services.GetRequiredService<IConfiguration>();
        //            var jwtKey = configuration["Jwt:Key"];
        //            var issuer = configuration["Jwt:Issuer"];
        //            var audience = configuration["Jwt:Audience"];

        //            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

        //            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //            //var token = new JwtSecurityToken(
        //            //    issuer: "MedicalSystem",
        //            //    audience: "medical_api",
        //            //    claims: claims,
        //            //    expires: DateTime.Now.AddHours(1),
        //            //    signingCredentials: creds);

        //            var token = new JwtSecurityToken(
        //issuer: issuer,
        //audience: audience,
        //claims: claims,
        //expires: DateTime.Now.AddHours(1),
        //signingCredentials: creds);

        //            return Results.Ok(new
        //            {
        //                token = new JwtSecurityTokenHandler().WriteToken(token)
        //            });
        //        }

        //        return Results.Unauthorized();
        //    }).AllowAnonymous();

        app.MapPost("/api/auth/login", async ([FromServices] AppDbContext db, [FromBody] LoginRequest request) =>
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return Results.Unauthorized();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Role, user.Role)
    };

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

            var role = string.IsNullOrEmpty(request.Role) ? UserRoles.User : request.Role;

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var user = new User
            {
                Username = request.Username,
                PasswordHash = hashedPassword,
                Role = role
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
                    Role = u.Role
                })
                .FirstOrDefaultAsync();

            return found is not null
                ? Results.Ok(found)
                : Results.NotFound("User not found");
        }).RequireAuthorization();
    }
}

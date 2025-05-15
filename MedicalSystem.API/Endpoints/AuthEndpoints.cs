using MedicalSystem.API.Models.Auth;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MedicalSystem.API.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        app.MapPost("/api/auth/login", ([FromBody] LoginRequest request) =>
        {
            // TODO: validate against real DB
            if (request.Username == "admin" && request.Password == "password")
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, request.Username),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisIsASuperLongSecretKeyWithAtLeast32Characters"));

                var configuration = app.Services.GetRequiredService<IConfiguration>();
                var jwtKey = configuration["Jwt:Key"];
                var issuer = configuration["Jwt:Issuer"];
                var audience = configuration["Jwt:Audience"];

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                //var token = new JwtSecurityToken(
                //    issuer: "MedicalSystem",
                //    audience: "medical_api",
                //    claims: claims,
                //    expires: DateTime.Now.AddHours(1),
                //    signingCredentials: creds);

                var token = new JwtSecurityToken(
    issuer: issuer,
    audience: audience,
    claims: claims,
    expires: DateTime.Now.AddHours(1),
    signingCredentials: creds);

                return Results.Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }

            return Results.Unauthorized();
        }).AllowAnonymous();
    }
}

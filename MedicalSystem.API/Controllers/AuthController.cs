using Azure.Core;
using MedicalSystem.API.Models.Auth;
using MedicalSystem.API.Services;
using MedicalSystem.Application.DTOs;
using MedicalSystem.Application.Models.Requests;
using MedicalSystem.Domain.Entities;
using MedicalSystem.Domain.Enums;
using MedicalSystem.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MedicalSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _config;
    private readonly TokenService _tokenService;

    public AuthController(AppDbContext db, IConfiguration config, TokenService tokenService)
    {
        _db = db;
        _config = config;
        _tokenService = tokenService;
    }

    private async Task<Guid> GetRoleIdAsync(string roleName)
    {
        var role = await _db.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
        if (role == null)
            throw new Exception($"Role '{roleName}' not found.");
        return role.Id;
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _db.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Username == request.Username);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return Unauthorized();

        //var claims = new List<Claim>
        //{
        //    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        //    new Claim(ClaimTypes.Name, user.Username)
        //};

        //foreach (var role in user.UserRoles.Select(ur => ur.Role.Name))
        //    claims.Add(new Claim(ClaimTypes.Role, role));

        //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        //var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //var token = new JwtSecurityToken(
        //    issuer: _config["Jwt:Issuer"],
        //    audience: _config["Jwt:Audience"],
        //    claims: claims,
        //    expires: DateTime.UtcNow.AddHours(1),
        //    signingCredentials: creds);

        //return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });

        //var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
        //if (user == null || user.PasswordHash != request.Password) // Replace with hash compare
        //    return Unauthorized();

        //var roles = await _db.UserRoles
        //    .Where(ur => ur.UserId == user.Id)
        //    .Select(ur => ur.Role.Name)
        //    .ToListAsync();

        var token = _tokenService.CreateToken(user, user.UserRoles.Select(ur => ur.Role.Name).ToList());
        return Ok(new LoginResponse { Token = token });
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var username = User.Identity?.Name;

        if (string.IsNullOrEmpty(username))
            return Unauthorized();

        var found = await _db.Users
            .Where(u => u.Username == username)
            .Select(u => new UserInfoResponse
            {
                Username = u.Username,
                UserRoles = u.UserRoles
            })
            .FirstOrDefaultAsync();

        return found is not null
            ? Ok(found)
            : NotFound("User not found");
    }
    
    [HttpPost("register-patient")]
    public async Task<IActionResult> RegisterPatient(RegisterPatientRequest request)
    {
        if (await _db.Users.AnyAsync(u => u.Username == request.Username))
            return BadRequest("Username already taken");

        var user = new User
        {
            Username = request.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            UserRoles = new List<UserRole>
            {
                new UserRole { RoleId = await GetRoleIdAsync(UserRoles.Patient) }
            }
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        _db.Patients.Add(new Patient
        {
            UserId = user.Id,
            DateOfBirth = request.DateOfBirth,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Gender = request.Gender
        });

        await _db.SaveChangesAsync();

        return Ok("Patient registered successfully");
    }

    [HttpPost("register-staff")]
    public async Task<IActionResult> RegisterStaff(RegisterStaffRequest request)
    {
        if (await _db.Users.AnyAsync(u => u.Username == request.Username))
            return BadRequest("Username already taken");

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            UserRoles = new List<UserRole>
            {
                new UserRole { RoleId = await GetRoleIdAsync(UserRoles.Doctor) }
            }
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        _db.StaffProfiles.Add(new StaffProfile
        {
            UserId = user.Id,
            Position = request.Role,
            Department = request.Department
        });

        await _db.SaveChangesAsync();

        return Ok("Doctor registered successfully");
    }

    //[HttpPost("register")]
    //public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    //{
    //    if (await _db.Users.AnyAsync(u => u.Username == request.Username))
    //        return BadRequest("Username already taken");

    //    var role = await _db.Roles.FirstOrDefaultAsync(r => r.Name == request.Role);
    //    if (role == null)
    //        return BadRequest("Invalid role");

    //    var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

    //    var user = new User
    //    {
    //        Username = request.Username,
    //        PasswordHash = hashedPassword,
    //        Email = request.Email
    //    };

    //    user.UserRoles.Add(new UserRole
    //    {
    //        Role = role
    //    });

    //    _db.Users.Add(user);
    //    await _db.SaveChangesAsync();

    //    return Ok(new { user.Id, user.Username, Role = request.Role });
    //}
}

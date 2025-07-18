
using MedicalSystem.API.Models.User;
using MedicalSystem.Application.Models.Requests;
using MedicalSystem.Domain.Entities;
using MedicalSystem.Domain.Enums;
using MedicalSystem.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicalSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _db;

        public UsersController(AppDbContext db)
        {
            _db = db;
        }        

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<UserListItem>>> GetAllUsers()
        {
            var users = await _db.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Select(u => new UserListItem
                {
                    Id = u.Id,
                    Username = u.Username,
                    Role = string.Join(", ", u.UserRoles.Select(ur => ur.Role.Name))
                })
                .ToListAsync();

            return Ok(users);
        }

        [HttpGet("roles")]
        public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
        {
            var users = await _db.Roles
                .Select(u => new 
                {
                    Id = u.Id,
                    Name = u.Name                    
                })
                .ToListAsync();

            return Ok(users);
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> RegisterPatient(AssignRoleRequest request)
        {
            var user = await _db.Users
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Id == request.UserId);

            if (user is null)
                return NotFound("User not found");

            var role = await _db.Roles.FirstOrDefaultAsync(r => r.Name == request.RoleName);
            if (role == null)
                return BadRequest("Invalid role");

            if (user.UserRoles != null)
            {
                var roleExists = user.UserRoles.FirstOrDefault(f => f.RoleId == role.Id);
                return BadRequest("Role already exists!");
            }
            
            user.UserRoles.Add(new Domain.Entities.UserRole
            {
                UserId = user.Id,
                RoleId = role.Id
            });

            await _db.SaveChangesAsync();

            return Ok($"User role updated to {request.RoleName}");
        }


        [HttpPut("{id:guid}/role")]
        public async Task<IActionResult> UpdateUserRole(Guid id, [FromBody] UpdateUserRoleRequest request)
        {
            var user = await _db.Users
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user is null)
                return NotFound("User not found");

            var role = await _db.Roles.FirstOrDefaultAsync(r => r.Name == request.Role);
            if (role == null)
                return BadRequest("Invalid role");

            // Remove existing roles and assign the new one
            user.UserRoles.Clear();
            user.UserRoles.Add(new Domain.Entities.UserRole
            {
                UserId = user.Id,
                RoleId = role.Id
            });

            await _db.SaveChangesAsync();

            return Ok($"User role updated to {request.Role}");
        }

        [HttpGet("doctors")]
        [Authorize(Roles = "Patient, Admin")]
        public async Task<IActionResult> GetDoctors()
        {
            var doctors = await _db.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).Where(u => u.UserRoles.Any(ur => ur.Role.Name == UserRoles.Doctor))
                                .Select(u => new
                                {
                                    u.Id,
                                    u.Username,
                                    Role = string.Join(", ", u.UserRoles.Select(ur => ur.Role.Name))
                                })
                                .ToListAsync();

            return Ok(doctors);
        }

    }
}
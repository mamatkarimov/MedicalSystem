using MedicalSystem.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicalSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("doctors")]
        [Authorize(Roles = "Patient, Admin")]
        public async Task<IActionResult> GetDoctors()
        {
            var doctors = await _context.Users
    .Include(u => u.UserRoles)
        .ThenInclude(ur => ur.Role)
    .Where(u => u.UserRoles.Any(ur => ur.Role.Name == "Doctor"))
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

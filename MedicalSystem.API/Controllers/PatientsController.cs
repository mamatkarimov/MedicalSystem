using MedicalSystem.Application.Models.Requests;
using MedicalSystem.Domain.Entities;
using MedicalSystem.Domain.Enums;
using MedicalSystem.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicalSystem.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PatientsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterPatientRequest request)
        {
            if (!string.IsNullOrEmpty(request.Username) && !string.IsNullOrEmpty(request.Password))
            {

                if (await _context.Users.AnyAsync(u => u.Username == request.Username))
                    return BadRequest("Username already taken");

                var user = new User
                {
                    Username = request.Username,
                    Email = request.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    UserRoles = new List<UserRole>
                        {
                            new UserRole { RoleId = await GetRoleIdAsync(UserRoles.Patient) }
                        }
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            var patient = new Patient
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender
            };

            if (!string.IsNullOrEmpty(request.Username))
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
                if (user != null)
                {
                    patient.UserId = user.Id;
                    patient.User = user;
                }
            }

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            return Ok(patient.Id);
        }

        private async Task<Guid> GetRoleIdAsync(string roleName)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            if (role == null)
                throw new Exception($"Role '{roleName}' not found.");
            return role.Id;
        }

        [HttpGet]
        //[AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var patients = await _context.Patients
                .Select(p => new
                {
                    p.Id,
                    p.FirstName,
                    p.LastName,
                    p.DateOfBirth,
                    p.Gender,
                    p.UserId,
                    p.User
                })
                .ToListAsync();

            return Ok(patients);
        }

        [Authorize(Roles = "Admin,Reception")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetPatient(Guid id)
        {
            var patient = await _context.Patients.FindAsync(id);

            if (patient == null || !patient.IsActive)
            {
                return NotFound();
            }

            return patient;
        }

        [Authorize(Roles = "Admin,Reception")]
        [HttpPost("register")]
        public async Task<ActionResult<Patient>> RegisterByReception(RegisterPatientRequest request)
        {
            // Same as self-register but without creating user account
            var patient = new Patient
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender
            };

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPatient", new { id = patient.Id }, patient);
        }

        [Authorize(Roles = "Admin,Reception")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(Guid id, Patient patient)
        {
            if (id != patient.Id)
            {
                return BadRequest();
            }

            _context.Entry(patient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(Guid id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            //patient.IsActive = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PatientExists(Guid id)
        {
            return _context.Patients.Any(e => e.Id == id);
        }
    }
}
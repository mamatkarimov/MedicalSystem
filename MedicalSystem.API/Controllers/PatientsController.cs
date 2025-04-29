using MedicalSystem.Application.Models.Requests;
using MedicalSystem.Domain.Entities;
using MedicalSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicalSystem.API.Controllers
{
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class PatientsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<MedicalSystem.Infrastructure.Identity.ApplicationUser> _userManager;

    public PatientsController(ApplicationDbContext context, UserManager<MedicalSystem.Infrastructure.Identity.ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [Authorize(Roles = "Admin,Reception")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Patient>>> GetPatients()
    {
        return await _context.Patients.Where(p => p.IsActive).ToListAsync();
    }

    [Authorize(Roles = "Admin,Reception")]
    [HttpGet("{id}")]
    public async Task<ActionResult<Patient>> GetPatient(int id)
    {
        var patient = await _context.Patients.FindAsync(id);

        if (patient == null || !patient.IsActive)
        {
            return NotFound();
        }

        return patient;
    }       

        [AllowAnonymous]
    [HttpPost("self-register")]
    public async Task<ActionResult<ApiResponse<Patient>>> SelfRegister(RegisterPatientRequest request)
    {
        // Validate model
        //if (!ModelState.IsValid)
        //    return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<PatientDto>(false, "Invalid data", ModelState));


            // Create patient
            var patient = new Patient
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            MiddleName = request.MiddleName,
            BirthDate = request.BirthDate,
            Gender = request.Gender,
            Address = request.Address,
            Phone = request.Phone,
            Email = request.Email,
            InsuranceNumber = request.InsuranceNumber,
            InsuranceCompany = request.InsuranceCompany
        };

        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();

        // If credentials provided, create user account
        if (!string.IsNullOrEmpty(request.Username) && !string.IsNullOrEmpty(request.Password))
        {
            var user = new MedicalSystem.Infrastructure.Identity.ApplicationUser
            {
                UserName = request.Username,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                MiddleName = request.MiddleName
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Patient");
            }
        }

        return CreatedAtAction("GetPatient", new { id = patient.PatientID }, patient);
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
            MiddleName = request.MiddleName,
            BirthDate = request.BirthDate,
            Gender = request.Gender,
            Address = request.Address,
            Phone = request.Phone,
            Email = request.Email,
            InsuranceNumber = request.InsuranceNumber,
            InsuranceCompany = request.InsuranceCompany
        };

        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetPatient", new { id = patient.PatientID }, patient);
    }

    [Authorize(Roles = "Admin,Reception")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePatient(int id, Patient patient)
    {
        if (id != patient.PatientID)
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
    public async Task<IActionResult> DeletePatient(int id)
    {
        var patient = await _context.Patients.FindAsync(id);
        if (patient == null)
        {
            return NotFound();
        }

        patient.IsActive = false;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool PatientExists(int id)
    {
        return _context.Patients.Any(e => e.PatientID == id);
    }
}
}
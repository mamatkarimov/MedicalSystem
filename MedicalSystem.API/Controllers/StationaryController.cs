
using MedicalSystem.Application.Models.Requests;
using MedicalSystem.Domain.Entities;
using MedicalSystem.Infrastructure.Data;
using MedicalSystem.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MedicalSystem.API.Controllers
{
    [Authorize]
[Route("api/[controller]")]
[ApiController]
public class StationaryController : ControllerBase
{
    private readonly AppDbContext _context;

    public StationaryController(AppDbContext context)
    {
        _context = context;
    }

    [Authorize(Roles = "Admin,ChiefDoctor")]
    [HttpGet("departments")]
    public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
    {
        return await _context.Departments
            .Include(d => d.HeadDoctor)
            .ToListAsync();
    }

    [Authorize(Roles = "Admin,ChiefDoctor,Nurse")]
    [HttpGet("wards")]
    public async Task<ActionResult<IEnumerable<Ward>>> GetWards()
    {
        return await _context.Wards
            .Include(w => w.Department)
            .Include(w => w.Beds)
            .ToListAsync();
    }

    [Authorize(Roles = "Admin,ChiefDoctor,Nurse")]
    [HttpGet("beds/available")]
    public async Task<ActionResult<IEnumerable<Bed>>> GetAvailableBeds()
    {
        return await _context.Beds
            .Include(b => b.Ward)
                .ThenInclude(w => w.Department)
            .Where(b => !b.IsOccupied)
            .ToListAsync();
    }

    [Authorize(Roles = "Admin,ChiefDoctor")]
    [HttpPost("admit")]
    public async Task<ActionResult<Hospitalization>> AdmitPatient(AdmitPatientRequest request)
    {
        var patient = await _context.Patients.FindAsync(request.PatientID);
        if (patient == null || !patient.IsActive)
        {
            return NotFound("Patient not found");
        }

        var bed = await _context.Beds.FindAsync(request.BedID);
        if (bed == null)
        {
            return NotFound("Bed not found");
        }

        if (bed.IsOccupied)
        {
            return BadRequest("Bed is already occupied");
        }

        var doctor = await _context.Users.FindAsync(request.AttendingDoctorID);
        if (doctor == null)
        {
            return NotFound("Doctor not found");
        }

        // Check if patient is already hospitalized
        var activeHospitalization = await _context.Hospitalizations
            .FirstOrDefaultAsync(h => h.PatientID == request.PatientID && h.Status == "Active");

        if (activeHospitalization != null)
        {
            return BadRequest("Patient is already hospitalized");
        }

        var hospitalization = new Hospitalization
        {
            PatientID = request.PatientID,
            BedID = request.BedID,
            AdmissionDate = DateTime.UtcNow,
            DiagnosisOnAdmission = request.DiagnosisOnAdmission,
            AttendingDoctorID = request.AttendingDoctorID,
            Status = "Active"
        };

        bed.IsOccupied = true;

        _context.Hospitalizations.Add(hospitalization);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetHospitalization", new { id = hospitalization.HospitalizationID }, hospitalization);
    }

    [Authorize(Roles = "Admin,ChiefDoctor")]
    [HttpPost("discharge/{id}")]
    public async Task<IActionResult> DischargePatient(int id, DischargePatientRequest request)
    {
        var hospitalization = await _context.Hospitalizations
            .Include(h => h.Bed)
            .FirstOrDefaultAsync(h => h.HospitalizationID == id);

        if (hospitalization == null)
        {
            return NotFound();
        }

        if (hospitalization.Status != "Active")
        {
            return BadRequest("Patient is not currently hospitalized");
        }

        hospitalization.DiagnosisOnDischarge = request.DiagnosisOnDischarge;
        hospitalization.DischargeDate = DateTime.UtcNow;
        hospitalization.Status = "Discharged";
        hospitalization.Bed.IsOccupied = false;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [Authorize(Roles = "Admin,ChiefDoctor,Nurse")]
    [HttpGet("hospitalizations/active")]
    public async Task<ActionResult<IEnumerable<Hospitalization>>> GetActiveHospitalizations()
    {
        return await _context.Hospitalizations
            .Include(h => h.Patient)
            .Include(h => h.Bed)
                .ThenInclude(b => b.Ward)
            .Include(h => h.AttendingDoctor)
            .Where(h => h.Status == "Active")
            .ToListAsync();
    }

    [Authorize(Roles = "Nurse")]
    [HttpPost("rounds")]
    public async Task<ActionResult<NurseRound>> AddNurseRound(NurseRound round)
    {
        var patient = await _context.Patients.FindAsync(round.PatientID);
        if (patient == null || !patient.IsActive)
        {
            return NotFound("Patient not found");
        }

        // Check if patient is hospitalized
        var hospitalization = await _context.Hospitalizations
            .FirstOrDefaultAsync(h => h.PatientID == round.PatientID && h.Status == "Active");

        if (hospitalization == null)
        {
            return BadRequest("Patient is not hospitalized");
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        round.NurseID = userId;

        _context.NurseRounds.Add(round);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetNurseRounds", new { patientId = round.PatientID }, round);
    }

    [Authorize(Roles = "Admin,ChiefDoctor,Nurse")]
    [HttpGet("rounds/{patientId}")]
    public async Task<ActionResult<IEnumerable<NurseRound>>> GetNurseRounds(int patientId)
    {
        return await _context.NurseRounds
            .Include(r => r.Nurse)
            .Where(r => r.PatientID == patientId)
            .OrderByDescending(r => r.RoundDate)
            .ToListAsync();
    }

    [Authorize(Roles = "Admin,ChiefDoctor,Nurse")]
    [HttpPost("diets")]
    public async Task<ActionResult<PatientDiet>> AddPatientDiet(PatientDiet diet)
    {
        var patient = await _context.Patients.FindAsync(diet.PatientID);
        if (patient == null || !patient.IsActive)
        {
            return NotFound("Patient not found");
        }

        // Check if patient is hospitalized
        var hospitalization = await _context.Hospitalizations
            .FirstOrDefaultAsync(h => h.PatientID == diet.PatientID && h.Status == "Active");

        if (hospitalization == null)
        {
            return BadRequest("Patient is not hospitalized");
        }

        diet.HospitalizationID = hospitalization.HospitalizationID;

        // End previous diet if exists
        var previousDiet = await _context.PatientDiets
            .FirstOrDefaultAsync(d => d.PatientID == diet.PatientID && d.EndDate == null);

        if (previousDiet != null)
        {
            previousDiet.EndDate = DateTime.UtcNow;
        }

        _context.PatientDiets.Add(diet);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetPatientDiets", new { patientId = diet.PatientID }, diet);
    }

    [Authorize(Roles = "Admin,ChiefDoctor,Nurse")]
    [HttpGet("diets/{patientId}")]
    public async Task<ActionResult<IEnumerable<PatientDiet>>> GetPatientDiets(int patientId)
    {
        return await _context.PatientDiets
            .Where(d => d.PatientID == patientId)
            .OrderByDescending(d => d.StartDate)
            .ToListAsync();
    }
}
}
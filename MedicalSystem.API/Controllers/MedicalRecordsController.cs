using MedicalSystem.Application.Models.Requests;
using MedicalSystem.Domain.Entities;
using MedicalSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MedicalSystem.API.Controllers
{
[Authorize(Roles = "Admin,Doctor")]
[Route("api/[controller]")]
[ApiController]
public class MedicalRecordsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public MedicalRecordsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("history/{patientId}")]
    public async Task<ActionResult<IEnumerable<MedicalHistory>>> GetPatientHistory(int patientId)
    {
        var patient = await _context.Patients.FindAsync(patientId);
        if (patient == null || !patient.IsActive)
        {
            return NotFound("Patient not found");
        }

        return await _context.MedicalHistories
            .Include(h => h.RecordedBy)
            .Where(h => h.PatientID == patientId)
            .OrderByDescending(h => h.RecordDate)
            .ToListAsync();
    }

    [HttpPost("history")]
    public async Task<ActionResult<MedicalHistory>> AddMedicalHistory(AddMedicalHistoryRequest request)
    {
        var patient = await _context.Patients.FindAsync(request.PatientID);
        if (patient == null || !patient.IsActive)
        {
            return NotFound("Patient not found");
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var history = new MedicalHistory
        {
            PatientID = request.PatientID,
            RecordedByID = userId,
            HistoryType = request.HistoryType,
            Description = request.Description
        };

        _context.MedicalHistories.Add(history);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetPatientHistory", new { patientId = history.PatientID }, history);
    }

    [HttpGet("prescriptions/{patientId}")]
    public async Task<ActionResult<IEnumerable<Prescription>>> GetPatientPrescriptions(int patientId)
    {
        var patient = await _context.Patients.FindAsync(patientId);
        if (patient == null || !patient.IsActive)
        {
            return NotFound("Patient not found");
        }

        return await _context.Prescriptions
            .Include(p => p.PrescribedBy)
            .Where(p => p.PatientID == patientId)
            .OrderByDescending(p => p.PrescriptionDate)
            .ToListAsync();
    }

    [HttpPost("prescriptions")]
    public async Task<ActionResult<Prescription>> CreatePrescription(CreatePrescriptionRequest request)
    {
        var patient = await _context.Patients.FindAsync(request.PatientID);
        if (patient == null || !patient.IsActive)
        {
            return NotFound("Patient not found");
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var prescription = new Prescription
        {
            PatientID = request.PatientID,
            PrescribedByID = userId,
            Medication = request.Medication,
            Dosage = request.Dosage,
            Frequency = request.Frequency,
            Duration = request.Duration,
            Instructions = request.Instructions
        };

        _context.Prescriptions.Add(prescription);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetPatientPrescriptions", new { patientId = prescription.PatientID }, prescription);
    }

    [HttpPut("prescriptions/{id}/complete")]
    public async Task<IActionResult> CompletePrescription(int id)
    {
        var prescription = await _context.Prescriptions.FindAsync(id);
        if (prescription == null)
        {
            return NotFound();
        }

        prescription.Status = "Completed";
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
}
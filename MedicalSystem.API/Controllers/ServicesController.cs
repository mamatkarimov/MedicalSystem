using MedicalSystem.API.Models.Patient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Doctor,Admin")]
    public class ServicesController : ControllerBase
    {
        // Assign service to patient (could create appointment)
        //[HttpPost("assign")]
        //public async Task<IActionResult> AssignService([FromBody] AssignServiceDto dto)
        //{
        //    // Create MedicalRecord entry
        //    // Optionally create Appointment
        //    // Return combined information
        //}

        //// Get services assigned to patient
        //[HttpGet("patient/{patientId}")]
        //public async Task<IActionResult> GetPatientServices(Guid patientId)
        //{
        //    // Return MedicalRecords where ServiceId is not null
        //}
    }
}
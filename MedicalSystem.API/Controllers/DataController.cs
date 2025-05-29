using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataController : ControllerBase
    {
        [Authorize(Roles = "Doctor")]
        [HttpGet("doctor")]
        public IActionResult DoctorData()
        {
            var username = User.Identity?.Name;
            return Ok($"Doctor-only data for {username}");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin")]
        public IActionResult AdminData()
        {
            return Ok("This is for Admins only.");
        }
    }
}

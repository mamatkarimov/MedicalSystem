using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(new { message = "Hello from API!" });
        }

        [HttpGet("secure")]
        [Authorize]
        public IActionResult GetSecure()
        {
            return new JsonResult(new { message = "Secure data", user = User.Identity.Name });
        }

        [HttpGet("admin")]
        [Authorize(Roles = "admin")]
        public IActionResult GetAdmin()
        {
            return new JsonResult(new { message = "Admin data", user = User.Identity.Name });
        }
    }
}

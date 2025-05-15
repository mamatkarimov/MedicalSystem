using MedicalSystem.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace MedicalSystem.Web.Controllers
{
    [Authorize(Roles = "Patient")]
    public class AppointmentController : Controller
    {
        private readonly IHttpClientFactory _factory;

        public AppointmentController(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var client = _factory.CreateClient("api");
            AddToken(client);

            var doctors = await client.GetFromJsonAsync<List<UserDto>>("/api/user/doctors");
            ViewBag.Doctors = doctors;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Guid doctorId, DateTime date, string symptoms)
        {
            Console.WriteLine($"doctorId: {doctorId}, date: {date}, symptoms: {symptoms}");

            var client = _factory.CreateClient("api");
            AddToken(client);

            var response = await client.PostAsJsonAsync("/api/appointment", new
            {
                DoctorId = doctorId,
                Date = date,
                Symptoms = symptoms
            });

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Failed to book appointment.";

                // ❗ Reload doctor list
                var doctors = await client.GetFromJsonAsync<List<UserDto>>("/api/user/doctors");
                ViewBag.Doctors = doctors;

                return View();
            }

            return RedirectToAction("Mine");
        }

        [HttpGet]
        public async Task<IActionResult> Mine()
        {
            var client = _factory.CreateClient("api");
            AddToken(client);

            var appointments = await client.GetFromJsonAsync<List<AppointmentDto>>("/api/appointment/MyAppointments");
            return View(appointments);
        }

        private void AddToken(HttpClient client)
        {
            var token = User.FindFirst("access_token")?.Value;
            if (token != null)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        [HttpGet]
        public async Task<IActionResult> MyAppointments()
        {
            var token = Request.Cookies["jwt"];
            var client = _factory.CreateClient("api");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("/api/appointment/mine");
            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Failed to load appointments.";
                return View(new List<AppointmentDto>());
            }

            var appointments = await response.Content.ReadFromJsonAsync<List<AppointmentDto>>();
            return View(appointments);
        }
    }
}

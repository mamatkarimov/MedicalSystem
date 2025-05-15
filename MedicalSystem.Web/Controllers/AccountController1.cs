using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

namespace MedicalSystem.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _http;

        public AccountController(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("api");
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var response = await _http.PostAsJsonAsync("/api/auth/login", new
            {
                username,
                password
            });

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Invalid login.";
                return View();
            }

            var result = await response.Content.ReadFromJsonAsync<JsonElement>();
            var token = result.GetProperty("token").GetString();

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            var claims = jwt.Claims.ToList();
            claims.Add(new Claim("access_token", token!));

            var identity = new ClaimsIdentity(claims, "MyCookie", ClaimTypes.Name, ClaimTypes.Role);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("MyCookie", principal);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookie");
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(string username, string password)
        {
            var response = await _http.PostAsJsonAsync("/api/auth/register", new
            {
                username,
                password,
                role = "Patient"
            });

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Registration failed.";
                return View();
            }

            return RedirectToAction("Login");
        }
    }
}

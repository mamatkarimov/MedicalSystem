using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using MedicalSystem.Staff.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicalSystem.Staff.Pages
{
    public partial class Login : ComponentBase
    {
        [Inject] private IHttpClientFactory HttpClientFactory { get; set; }
        [Inject] private NavigationManager Navigation { get; set; }
        [Inject] private IHttpContextAccessor HttpContextAccessor { get; set; }

        private string Username { get; set; } = string.Empty;
        private string Password { get; set; } = string.Empty;
        private string ErrorMessage { get; set; }

        private async Task HandleLoginAsync()
        {
            var loginRequest = new
            {
                username = Username,
                password = Password
            };

            var httpClient = HttpClientFactory.CreateClient();
            var content = new StringContent(JsonSerializer.Serialize(loginRequest), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("http://localhost:5074/api/auth/login", content); // Adjust API URL

            if (!response.IsSuccessStatusCode)
            {
                ErrorMessage = "Invalid username or password.";
                return;
            }

            var responseString = await response.Content.ReadAsStringAsync();
            var loginResponse = JsonSerializer.Deserialize<LoginResponse>(responseString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (loginResponse == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                ErrorMessage = "Invalid response from server.";
                return;
            }

            // Parse JWT and create claims
            var claims = JwtParser.ExtractClaims(loginResponse.Token);

            // Add token as a claim (optional)
            claims.Add(new Claim("access_token", loginResponse.Token));

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
            };

            await HttpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

            Navigation.NavigateTo("/");
        }

        private class LoginResponse
        {
            public string Token { get; set; }
        }
    }
}

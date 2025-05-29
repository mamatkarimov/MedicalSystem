using MedicalSystem.Application.Models.Responses;
using Newtonsoft.Json;

namespace MedicalSystem.Staff.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<AuthResponse?> Login(AuthRequest request)
        {
            var result = await _httpClient.PostAsJsonAsync("api/auth/login", request);
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<AuthResponse>(content);
            }
            else
            {
                return null;
            }
        }
    }

    public class AuthRequest
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }

    public record AuthResponse
    {
        public string? UserId { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public List<string>? Roles { get; set; }
        public string? Token { get; set; }
    }
}

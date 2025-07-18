using MedicalSystem.Application.DTOs;
using MedicalSystem.Staff.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MedicalSystem.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authProvider;

        public AuthService(HttpClient httpClient, AuthenticationStateProvider authProvider)
        {
            _httpClient = httpClient;
            _authProvider = authProvider;
        }

        public async Task<bool> Login(string username, string password)
        {
            var response = await _httpClient.PostAsJsonAsync("http://localhost:5074/api/auth/login", new LoginRequest
            {
                Username = username,
                Password = password
            });

            if (!response.IsSuccessStatusCode)
                return false;

            var loginResult = await response.Content.ReadFromJsonAsync<LoginResponse>();

            if (loginResult == null || string.IsNullOrEmpty(loginResult.Token))
                return false;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResult.Token);

            await ((ApiAuthenticationStateProvider)_authProvider).MarkUserAsAuthenticated(username, loginResult.Token, loginResult.Roles.ToArray<string>());
            return true;
        }
    }
}
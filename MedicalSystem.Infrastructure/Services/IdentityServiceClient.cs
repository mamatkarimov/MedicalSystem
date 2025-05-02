using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystem.Infrastructure.Services
{
    public class IdentityServiceClient : IIdentityServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<IdentityServiceClient> _logger;

        public IdentityServiceClient(
            HttpClient httpClient,
            ILogger<IdentityServiceClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IEnumerable<string>> GetAllUsersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/users/ids");
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<IEnumerable<string>>()
                       ?? Enumerable.Empty<string>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch user IDs from IdentityService");
                throw;
            }
        }

        public async Task<IdentityUserInfo> GetUserAsync(string userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/users/{userId}");
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<IdentityUserInfo>()
                       ?? throw new InvalidOperationException("Null user response");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch user {UserId} from IdentityService", userId);
                throw;
            }
        }
    }

    // DTOs
    public record IdentityUserInfo(
        string Id,
        string Email,
        string FirstName,
        string LastName,
        bool IsActive);

    // Interface
    public interface IIdentityServiceClient
    {
        Task<IEnumerable<string>> GetAllUsersAsync();
        Task<IdentityUserInfo> GetUserAsync(string userId);
    }
}

using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Threading.Tasks;

namespace MedicalSystem.Staff.Services
{
    public class SecureStorageService
    {
        private const string TokenKey = "authToken";

        private readonly ProtectedSessionStorage _sessionStorage;

        public SecureStorageService(ProtectedSessionStorage sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        public async Task SetToken(string token)
        {
            await _sessionStorage.SetAsync(TokenKey, token);
        }

        public async Task<string?> GetToken()
        {
            var result = await _sessionStorage.GetAsync<string>(TokenKey);
            return result.Success ? result.Value : null;
        }

        public async Task ClearToken()
        {
            await _sessionStorage.DeleteAsync(TokenKey);
        }
    }
}

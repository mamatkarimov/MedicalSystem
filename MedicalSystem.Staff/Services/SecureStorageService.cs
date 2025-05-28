using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Threading.Tasks;

namespace MedicalSystem.Staff.Services
{
    public class TokenService
    {
        private const string TokenKey = "jwt_token";
        private readonly ProtectedLocalStorage _protectedLocalStorage;
        public TokenService(ProtectedLocalStorage protectedLocalStorage)
        {
            _protectedLocalStorage = protectedLocalStorage;
        }
        public Task SaveTokenAsync(string token)
        {
            await _protectedLocalStorage.SetAsync("jwt_token", token);
            var result = await _protectedLocalStorage.GetAsync<string>("jwt_token");
            var token = result.Success ? result.Value : null;
        }

        public async Task<string> GetTokenAsync()
        {
            var result = await _protectedLocalStorage.GetAsync<string>(TokenKey);
            return result.Success ? result.Value : null;
        }

        public ValueTask RemoveTokenAsync()
        {
            return _protectedLocalStorage.DeleteAsync(TokenKey);
        }

        //[Inject] public ProtectedLocalStorage ProtectedLocalStorage { get; set; }
    }
}

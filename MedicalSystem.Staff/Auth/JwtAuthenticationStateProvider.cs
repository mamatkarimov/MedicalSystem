using MedicalSystem.Staff.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
namespace MedicalSystem.Staff.Auth
{
    public class JwtAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly TokenService _storageService;
        private Timer? _expirationTimer;

        public JwtAuthenticationStateProvider(TokenService storageService)
        {
            _storageService = storageService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _storageService.GetTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var claims = ParseClaimsFromJwt(token);

            var exp = GetExpiryFromToken(token);
            if (exp != null && exp <= DateTime.UtcNow)
            {
                await Logout();
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            StartExpirationTimer(exp);

            var identity = new ClaimsIdentity(claims, "jwt");
            var user = new ClaimsPrincipal(identity);
            return new AuthenticationState(user);
        }

        public void NotifyUserAuthentication(string token)
        {
            var claims = ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "jwt");
            var user = new ClaimsPrincipal(identity);

            var exp = GetExpiryFromToken(token);
            StartExpirationTimer(exp);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public async Task Logout()
        {
            _expirationTimer?.Dispose();
            await _storageService.RemoveTokenAsync();
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))));
        }

        private void StartExpirationTimer(DateTime? expiry)
        {
            _expirationTimer?.Dispose();

            if (expiry == null)
                return;

            var timeUntilExpiry = expiry.Value - DateTime.UtcNow;

            if (timeUntilExpiry <= TimeSpan.Zero)
            {
                // Already expired
                _ = Logout();
                return;
            }

            _expirationTimer = new Timer(async _ =>
            {
                await Logout();
            }, null, timeUntilExpiry, Timeout.InfiniteTimeSpan);
        }

        private static DateTime? GetExpiryFromToken(string jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);
            var expClaim = token.Payload.Expiration;
            if (expClaim == null) return null;

            var expDateTime = DateTimeOffset.FromUnixTimeSeconds((long)expClaim).UtcDateTime;
            return expDateTime;
        }

        private static Claim[] ParseClaimsFromJwt(string jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);
            return token.Claims.ToArray();
        }

        public void NotifyUserLogout()
        {
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))));
        }
    }
}

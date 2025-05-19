using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MedicalSystem.Staff.Auth
{
    public static class JwtParser
    {
        public static List<Claim> ExtractClaims(string jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);

            var claims = token.Claims.ToList();

            // Keycloak roles (realm_access)
            var realmAccessClaim = claims.FirstOrDefault(c => c.Type == "realm_access");
            if (realmAccessClaim != null)
            {
                var json = System.Text.Json.JsonDocument.Parse(realmAccessClaim.Value);
                if (json.RootElement.TryGetProperty("roles", out var roles))
                {
                    foreach (var role in roles.EnumerateArray())
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role.GetString()));
                    }
                }
            }

            return claims;
        }
    }
}

using IdentityServer4.Models;
using IdentityServer4;

namespace MedicalSystem.AuthService
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email(),
            new IdentityResource("roles", "User roles", new[] { "role" }),
            new IdentityResource("permissions", "User permissions", new[] { "permission" })
        };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
        {
            new ApiScope("clinicapi", "ClinicHub API")
        };
        }

        public static IEnumerable<Client> GetClients(IConfiguration config)
        {
            return new List<Client>
        {
            new Client
            {
                ClientId = "clinicweb",
                ClientName = "ClinicHub Web",
                AllowedGrantTypes = GrantTypes.Code,
                RequireClientSecret = true,
                ClientSecrets = { new Secret(config["ClientSecrets:Web"].Sha256()) },
                RedirectUris = { config["Clients:Web:RedirectUri"] },
                PostLogoutRedirectUris = { config["Clients:Web:PostLogoutRedirectUri"] },
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    "roles",
                    "permissions",
                    "clinicapi"
                },
                AllowOfflineAccess = true,
                RefreshTokenUsage = TokenUsage.OneTimeOnly,
                RefreshTokenExpiration = TokenExpiration.Sliding,
                AccessTokenLifetime = 3600, // 1 hour
                AbsoluteRefreshTokenLifetime = 86400 // 1 day
            }
        };
        }
    }
}

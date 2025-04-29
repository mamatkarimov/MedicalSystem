using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace MedicalSystem.AuthService
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource("roles", "User roles", new[] { "role" }),
                new IdentityResource("permissions", "User permissions", new[] { "permission" })
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("medicalapi", "Medical System API")
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
                new ApiResource("medicalapi", "Medical System API")
                {
                    Scopes = { "medicalapi" },
                    UserClaims = { "role", "permission" }
                }
            };

        public static IEnumerable<Client> Clients(IConfiguration config) =>
            new List<Client>
            {
                // Web Client
                new Client
                {
                    ClientId = "medicalweb",
                    ClientName = "Medical System Web",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = true,
                    ClientSecrets = { new Secret(config["Clients:Web:Secret"].Sha256()) },

                    RedirectUris = { config["Clients:Web:RedirectUri"] },
                    PostLogoutRedirectUris = { config["Clients:Web:PostLogoutRedirectUri"] },
                    FrontChannelLogoutUri = config["Clients:Web:FrontChannelLogoutUri"],

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "roles",
                        "permissions",
                        "medicalapi",
                        "offline_access"
                    },

                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.ReUse,
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    AccessTokenLifetime = 3600, // 1 hour
                    AbsoluteRefreshTokenLifetime = 86400, // 1 day
                    UpdateAccessTokenClaimsOnRefresh = true,
                    
                    // Required for Blazor WASM
                    AllowedCorsOrigins = { config["Clients:Web:Origin"] }
                },

                // API-to-API Client
                new Client
                {
                    ClientId = "medicalservice",
                    ClientName = "Medical System Service",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret(config["Clients:Service:Secret"].Sha256()) },
                    AllowedScopes = { "medicalapi" },
                    AccessTokenLifetime = 3600 // 1 hour
                }
            };

        public static IEnumerable<IdentityResource> GetIdentityResources()
            => IdentityResources;

        public static IEnumerable<ApiResource> GetApiResources()
            => ApiResources;

        public static IEnumerable<ApiScope> GetApiScopes()
            => ApiScopes;

        public static IEnumerable<Client> GetClients(IConfiguration config)
            => Clients(config);
    }
}
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;


namespace MedicalSystem.AuthService.Config
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("api1", "My API")
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // MVC Client (Hybrid Flow)
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = IdentityServer4.Models.GrantTypes.Hybrid,

                    RedirectUris = { "https://localhost:5002/signin-oidc" },
                    PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "api1"
                    },

                    AllowOfflineAccess = true,
                    RequireConsent = false
                },
                
                // API Client (Client Credentials)
                new Client
                {
                    ClientId = "api_client",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "api1" }
                }
            };
    }
}

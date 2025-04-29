using IdentityServer4.Test;
using System.Security.Claims;

namespace MedicalSystem.AuthService
{
    public static class TestUsers
    {
        public static List<TestUser> Users = new List<TestUser>
        {
            new TestUser
            {
                SubjectId = "818727", // Unique identifier
                Username = "alice",
                Password = "alice", // In production, use hashed passwords!
                Claims = new List<Claim>
                {
                    new Claim("name", "Alice Smith"),
                    new Claim("given_name", "Alice"),
                    new Claim("family_name", "Smith"),
                    new Claim("email", "alice@example.com"),
                    new Claim("email_verified", "true", ClaimValueTypes.Boolean),
                    new Claim("website", "http://alice.com"),
                    new Claim("role", "admin"),
                    new Claim("role", "user") // Multiple roles example
                }
            },
            new TestUser
            {
                SubjectId = "88421113",
                Username = "bob",
                Password = "bob",
                Claims = new List<Claim>
                {
                    new Claim("name", "Bob Jones"),
                    new Claim("given_name", "Bob"),
                    new Claim("family_name", "Jones"),
                    new Claim("email", "bob@example.com"),
                    new Claim("email_verified", "true", ClaimValueTypes.Boolean),
                    new Claim("role", "user")
                }
            }
        };
    }
}

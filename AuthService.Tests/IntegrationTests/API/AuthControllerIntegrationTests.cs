using AuthService.API;
using AuthService.API.Controllers;
using AuthService.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace AuthService.Tests.IntegrationTests.API
{
    public class AuthControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public AuthControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Replace with test database
                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("TestAuthDB");
                    });
                });
            });
        }

        [Fact]
        public async Task Login_WithValidUser_ReturnsToken()
        {
            // Arrange
            var client = _factory.CreateClient();
            var loginRequest = new
            {
                email = "test@example.com",
                password = "ValidPass123!"
            };

            // Seed test user
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.Users.Add(TestUserFactory.CreateValidUser());
            await db.SaveChangesAsync();

            // Act
            var response = await client.PostAsJsonAsync("/api/auth/login", loginRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadFromJsonAsync<AuthResponse>();
            content.Token.Should().NotBeNullOrEmpty();
        }
    }
}
using AuthService.Core.Entities;
using AuthService.Core.Interfaces;
using AuthService.Core.Services;
using AuthService.Shared.DTOs.Auth;
using AuthService.Tests.TestHelpers.Factories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AuthService.Tests.UnitTests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _userRepoMock = new();
        private readonly Mock<ITokenService> _tokenServiceMock = new();
        private readonly AuthService.Core.Services.AuthService _authService;

        public AuthServiceTests()
        {
            _authService = new AuthService.Core.Services.AuthService(
                _userRepoMock.Object,
                _tokenServiceMock.Object,
                Mock.Of<ILogger<AuthService.Core.Services.AuthService>>());
        }

        [Fact]
        public async Task LoginAsync_WithValidCredentials_ReturnsAuthResponse()
        {
            // Arrange
            var request = new LoginRequest { Email = "test@example.com", Password = "ValidPass123!" };
            var user = TestUserFactory.CreateValidUser();

            _userRepoMock.Setup(x => x.GetByEmailAsync(request.Email))
                .ReturnsAsync(user);

            _tokenServiceMock.Setup(x => x.ValidatePassword(It.IsAny<User>(), request.Password))
                .ReturnsAsync(true);

            // Act
            var result = await _authService.LoginAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Token.Should().NotBeNullOrEmpty();
        }

        [Theory]
        [InlineData(null, "password")]
        [InlineData("email@test.com", null)]
        public async Task LoginAsync_WithMissingCredentials_ReturnsFailure(string email, string password)
        {
            // Arrange
            var request = new LoginRequest { Email = email, Password = password };

            // Act
            var result = await _authService.LoginAsync(request);

            // Assert
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain("Invalid credentials");
        }
    }
}
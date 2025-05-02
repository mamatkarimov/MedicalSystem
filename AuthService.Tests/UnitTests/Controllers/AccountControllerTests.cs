using AuthService.API.Controllers;
using AuthService.Core.Interfaces;
using AuthService.Shared.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AuthService.Tests.UnitTests.Controllers
{
    public class AccountControllerTests
    {
        private readonly Mock<IAuthService> _authServiceMock = new();
        private readonly AccountController _controller;

        public AccountControllerTests()
        {
            _controller = new AccountController(
                _authServiceMock.Object,
                Mock.Of<ILogger<AccountController>>());
        }

        [Fact]
        public async Task Register_ValidRequest_ReturnsOk()
        {
            // Arrange
            var request = new RegisterRequest
            {
                Email = "test@example.com",
                Password = "ValidPass123!",
                FirstName = "Test",
                LastName = "User"
            };

            _authServiceMock.Setup(x => x.RegisterAsync(request))
                .ReturnsAsync(new AuthResponse { Success = true });

            // Act
            var result = await _controller.Register(request);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }
    }
}
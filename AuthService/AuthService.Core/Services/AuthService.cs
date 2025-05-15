using AuthService.Core.Entities;
using AuthService.Core.Interfaces;
using AuthService.Shared.DTOs.Auth;
using AuthService.Shared.DTOs.User;
using AuthService.Shared.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthService.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IDeviceRepository _deviceRepository;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ITokenService tokenService,
            IRefreshTokenRepository refreshTokenRepository,
            IDeviceRepository deviceRepository,
            ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _refreshTokenRepository = refreshTokenRepository;
            _deviceRepository = deviceRepository;
            _logger = logger;
        }

        public async Task<AuthenticationResult> RegisterAsync(RegisterRequest request)
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(request.Email);
                if (existingUser != null)
                {
                    return new AuthenticationResult
                    {
                        Success = false,
                        Message = "Email already exists"
                    };
                }

                var newUser = new User
                {
                    Email = request.Email,
                    UserName = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    CreatedDate = DateTimeOffset.UtcNow,
                    IsActive = true
                };

                var hashedPassword = PasswordHasher.Hash(request.Password);
                var createResult = await _userManager.CreateAsync(newUser, hashedPassword);
                if (!createResult.Succeeded)
                {
                    return new AuthenticationResult
                    {
                        Success = false,
                        Message = "User creation failed",
                        Errors = createResult.Errors.Select(x => x.Description).ToList()
                    };
                }

                // Add to default role if specified
                if (!string.IsNullOrEmpty(request.DefaultRole))
                {
                    await _userManager.AddToRoleAsync(newUser, request.DefaultRole);
                }

                _logger.LogInformation("New user registered: {Email}", request.Email);

                // Generate tokens without requiring login
                var tokenResult = _tokenService.GenerateJwtToken(newUser);
                var refreshToken = _tokenService.GenerateRefreshToken(
                    newUser,
                    request.IpAddress,
                    request.UserAgent,
                    request.DeviceId);

                await _refreshTokenRepository.AddAsync(refreshToken);

                return new AuthenticationResult
                {
                    Success = true,
                    Token = tokenResult.Token,
                    RefreshToken = refreshToken.Token,
                    TokenExpiration = tokenResult.Expiration,
                    User = MapToUserDto(newUser)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user registration");
                throw;
            }
        }

        public async Task<AuthenticationResult> LoginAsync(LoginRequest request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    return new AuthenticationResult
                    {
                        Success = false,
                        Message = "Invalid credentials"
                    };
                }

                // Check account status
                if (!user.IsActive)
                {
                    return new AuthenticationResult
                    {
                        Success = false,
                        Message = "Account is disabled"
                    };
                }

                if (await _userManager.IsLockedOutAsync(user))
                {
                    return new AuthenticationResult
                    {
                        Success = false,
                        Message = "Account is locked out"
                    };
                }

                var hashedPassword = PasswordHasher.Hash(request.Password);

                var signInResult = await _signInManager.CheckPasswordSignInAsync(user, hashedPassword, false);
                if (!signInResult.Succeeded)
                {
                    // Track failed attempts
                    await _userManager.AccessFailedAsync(user);

                    return new AuthenticationResult
                    {
                        Success = false,
                        Message = signInResult.IsLockedOut ? "Account locked due to multiple failed attempts" : "Invalid credentials"
                    };
                }

                // Reset access failed count on successful login
                await _userManager.ResetAccessFailedCountAsync(user);

                // Update last login
                user.LastLoginDate = DateTimeOffset.UtcNow;
                await _userManager.UpdateAsync(user);

                // Register device if not exists
                if (!string.IsNullOrEmpty(request.DeviceId))
                {
                    await RegisterDeviceIfNotExists(user, request.DeviceId, request.IpAddress, request.UserAgent);
                }

                // Generate tokens
                var tokenResult = _tokenService.GenerateJwtToken(user);
                var refreshToken = _tokenService.GenerateRefreshToken(
                    user,
                    request.IpAddress,
                    request.UserAgent,
                    request.DeviceId);

                await _refreshTokenRepository.AddAsync(refreshToken);

                _logger.LogInformation("User logged in: {Email}", request.Email);

                return new AuthenticationResult
                {
                    Success = true,
                    Token = tokenResult.Token,
                    RefreshToken = refreshToken.Token,
                    TokenExpiration = tokenResult.Expiration,
                    User = MapToUserDto(user)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user login");
                throw;
            }
        }

        public async Task<AuthenticationResult> RefreshTokenAsync(TokenRequest request)
        {
            try
            {
                var principal = _tokenService.GetPrincipalFromToken(request.Token);
                if (principal == null)
                {
                    return new AuthenticationResult { Success = false, Message = "Invalid token" };
                }

                var jti = principal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.Value;
                var userIdClaim = principal.Claims.FirstOrDefault(x => x.Type == TokenClaims.UserId);

                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
                {
                    return new AuthenticationResult { Success = false, Message = "Invalid token" };
                }

                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null || !user.IsActive)
                {
                    return new AuthenticationResult { Success = false, Message = "User not found or inactive" };
                }

                var storedRefreshToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken);
                if (storedRefreshToken == null ||
                    storedRefreshToken.UserId != userId ||
                    storedRefreshToken.JwtId != jti ||
                    !storedRefreshToken.IsActive)
                {
                    return new AuthenticationResult { Success = false, Message = "Invalid refresh token" };
                }

                // Revoke used refresh token
                storedRefreshToken.IsUsed = true;
                await _refreshTokenRepository.UpdateAsync(storedRefreshToken);

                // Generate new tokens
                var tokenResult = _tokenService.GenerateJwtToken(user);
                var newRefreshToken = _tokenService.GenerateRefreshToken(
                    user,
                    storedRefreshToken.IpAddress,
                    storedRefreshToken.UserAgent,
                    storedRefreshToken.DeviceIdentifier);

                await _refreshTokenRepository.AddAsync(newRefreshToken);

                return new AuthenticationResult
                {
                    Success = true,
                    Token = tokenResult.Token,
                    RefreshToken = newRefreshToken.Token,
                    TokenExpiration = tokenResult.Expiration,
                    User = MapToUserDto(user)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during token refresh");
                throw;
            }
        }

        public async Task<RevocationResult> RevokeTokenAsync(TokenRequest request)
        {
            try
            {
                var principal = _tokenService.GetPrincipalFromToken(request.Token);
                if (principal == null)
                {
                    return new RevocationResult { Success = false, Message = "Invalid token" };
                }

                var jti = principal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.Value;
                var userIdClaim = principal.Claims.FirstOrDefault(x => x.Type == TokenClaims.UserId);

                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
                {
                    return new RevocationResult { Success = false, Message = "Invalid token" };
                }

                var storedRefreshToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken);
                if (storedRefreshToken == null || storedRefreshToken.UserId != userId)
                {
                    return new RevocationResult { Success = false, Message = "Invalid refresh token" };
                }

                // Revoke both tokens
                storedRefreshToken.IsRevoked = true;
                await _refreshTokenRepository.UpdateAsync(storedRefreshToken);

                await _tokenService.RevokeTokenAsync(jti, storedRefreshToken.IpAddress);

                return new RevocationResult
                {
                    Success = true,
                    TokensRevoked = 1,
                    Message = "Tokens revoked successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during token revocation");
                throw;
            }
        }

        // Additional helper methods
        private async Task RegisterDeviceIfNotExists(User user, string deviceId, string ipAddress, string userAgent)
        {
            var existingDevice = await _deviceRepository.GetByDeviceIdAsync(user.Id, deviceId);
            if (existingDevice == null)
            {
                var newDevice = new Device
                {
                    UserId = user.Id,
                    
                    Id = deviceId,
                    DeviceName = userAgent, // Or extract from User-Agent
                    LastLoginDate = DateTime.Now,
                    IpAddress = ipAddress,
                    IsTrusted = false // Require additional verification for trusted status
                };
                await _deviceRepository.AddAsync(newDevice);
            }
        }

        private UserDto MapToUserDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ProfilePictureUrl = user.ProfilePictureUrl,
                IsActive = user.IsActive,
                TwoFactorEnabled = user.TwoFactorEnabled
            };
        }

        public Task<RevocationResult> RevokeAllRefreshTokensForUserAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<PasswordChangeResult> ChangePasswordAsync(string userId, ChangePasswordRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<PasswordResetResult> RequestPasswordResetAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<PasswordResetResult> ResetPasswordAsync(ResetPasswordRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<TwoFactorResult> EnableTwoFactorAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<TwoFactorResult> DisableTwoFactorAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<TwoFactorResult> VerifyTwoFactorAsync(TwoFactorVerificationRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<VerificationResult> SendEmailVerificationAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<AccountStatusResult> LockAccountAsync(string userId, LockAccountRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<AccountStatusResult> UnlockAccountAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<AccountStatusResult> DisableAccountAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<AccountStatusResult> EnableAccountAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<DeviceResult> RegisterDeviceAsync(string userId, RegisterDeviceRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<DeviceResult> RevokeDeviceAsync(string userId, string deviceId)
        {
            throw new NotImplementedException();
        }

        public Task<DeviceListResult> GetUserDevicesAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<SessionListResult> GetActiveSessionsAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<SessionResult> TerminateSessionAsync(string userId, string sessionId)
        {
            throw new NotImplementedException();
        }

        public Task<SessionResult> TerminateAllSessionsAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<SessionResult> GetUserProfileAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
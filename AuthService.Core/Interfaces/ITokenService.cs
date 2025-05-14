using AuthService.Core.Entities;
using AuthService.Shared.DTOs.Auth;
using AuthService.Shared.DTOs.Roles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthService.Core.Interfaces
{
    public interface ITokenService
    {
        // Token Generation
        TokenGenerationResult GenerateJwtToken(User user);
        TokenGenerationResult GenerateJwtToken(IEnumerable<Claim> claims);
        RefreshToken GenerateRefreshToken(User user, string ipAddress, string userAgent, string deviceIdentifier);

        // Token Validation
        TokenValidationResult ValidateToken(string token);
        TokenValidationResult ValidateRefreshToken(string token);

        // Token Management
        Task<TokenRevocationResult> RevokeTokenAsync(string token, string ipAddress);
        Task<TokenRevocationResult> RevokeAllTokensForUserAsync(Guid userId);
        Task<bool> IsTokenRevokedAsync(string jti);

        // Special Token Operations
        TokenGenerationResult GeneratePasswordResetToken(User user);
        TokenGenerationResult GenerateEmailConfirmationToken(User user);
        TokenGenerationResult GenerateTwoFactorToken(User user, string provider);

        // Claims Processing
        IEnumerable<Claim> GetUserClaims(User user);
        IEnumerable<Claim> GetRoleClaims(IEnumerable<string> roles);
        ClaimsPrincipal GetPrincipalFromToken(string token);

        // Security Stamp Validation
        Task<bool> ValidateSecurityStampAsync(User user, string securityStamp);

        // Token Configuration
        int GetRefreshTokenLifetimeDays();
        int GetTokenLifetimeMinutes();
        Task<SessionResult> ChangePasswordAsync(string userId, ChangePasswordRequest request);
    }

}
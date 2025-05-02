using AuthService.Core.Entities;
using AuthService.Core.Interfaces;
using AuthService.Shared.DTOs;
using AuthService.Shared.DTOs.Auth;
using AuthService.Shared.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Core.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<TokenService> _logger;

        public TokenService(
            IOptions<JwtSettings> jwtSettings,
            UserManager<User> userManager,
            ILogger<TokenService> logger)
        {
            _jwtSettings = jwtSettings.Value;
            _userManager = userManager;
            _logger = logger;
        }

        public TokenGenerationResult GenerateJwtToken(User user)
        {
            var claims = GetUserClaims(user).Result;
            return GenerateJwtToken(claims);
        }

        public TokenGenerationResult GenerateJwtToken(IEnumerable<Claim> claims)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var jwtId = Guid.NewGuid().ToString();

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Issuer = _jwtSettings.Issuer,
                    Audience = _jwtSettings.Audience,
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
                    SigningCredentials = credentials,
                    IssuedAt = DateTime.UtcNow,
                    NotBefore = DateTime.UtcNow
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return new TokenGenerationResult
                {
                    Success = true,
                    Token = tokenString,
                    Expiration = token.ValidTo,
                    Jti = jwtId,
                    Claims = claims
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating JWT token");
                return new TokenGenerationResult
                {
                    Success = false,
                    Message = "Error generating token",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public RefreshToken GenerateRefreshToken(User user, string ipAddress, string userAgent, string deviceIdentifier)
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            var a=  new RefreshToken(token: Convert.ToBase64String(randomNumber), jwtId: Guid.NewGuid().ToString(), userId: user.Id,
                ipAddress: ipAddress,
                userAgent: userAgent,
                deviceIdentifier: deviceIdentifier,
                expiryDate: DateTimeOffset.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays)) { CreatedDate = DateTimeOffset.UtcNow }
            ;
            return a;
        }

        public Shared.DTOs.Auth.TokenValidationResult ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = GetValidationParameters();

                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;

                return new Shared.DTOs.Auth.TokenValidationResult
                {
                    IsValid = true,
                    Principal = principal,
                    Claims = jwtToken.Claims
                };
            }
            catch (SecurityTokenExpiredException ex)
            {
                _logger.LogWarning("Expired token: {token}", token);
                return new Shared.DTOs.Auth.TokenValidationResult
                {
                    IsValid = false,
                    IsExpired = true,
                    FailureReason = "Token expired",
                    Claims = GetClaimsWithoutValidation(token)
                };
            }
            catch (SecurityTokenValidationException ex)
            {
                _logger.LogWarning("Invalid token: {token} - {message}", token, ex.Message);
                return new Shared.DTOs.Auth.TokenValidationResult
                {
                    IsValid = false,
                    FailureReason = "Invalid token"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating token");
                return new Shared.DTOs.Auth.TokenValidationResult
                {
                    IsValid = false,
                    FailureReason = "Error validating token"
                };
            }
        }

        public async Task<bool> ValidateSecurityStampAsync(User user, string securityStamp)
        {
            return await _userManager.VerifyUserTokenAsync(
                user,
                _userManager.Options.Tokens.ChangeEmailTokenProvider,
                "SecurityStamp",
                securityStamp);
        }

        public ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var result = ValidateToken(token);
            return result.IsValid ? result.Principal : null;
        }

        public async Task<IEnumerable<Claim>> GetUserClaims(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(TokenClaims.UserId, user.Id.ToString()),
                new Claim(TokenClaims.SecurityStamp, await _userManager.GetSecurityStampAsync(user)),
                new Claim(TokenClaims.TokenType, TokenTypes.AccessToken)
            };

            // Add roles
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Add custom claims
            claims.AddRange(await _userManager.GetClaimsAsync(user));

            return claims;
        }

        public IEnumerable<Claim> GetRoleClaims(IEnumerable<string> roles)
        {
            var claims = new List<Claim>();
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }

        public int GetRefreshTokenLifetimeDays() => _jwtSettings.RefreshTokenExpirationDays;
        public int GetTokenLifetimeMinutes() => _jwtSettings.AccessTokenExpirationMinutes;

        private TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = _jwtSettings.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true
            };
        }

        private IEnumerable<Claim> GetClaimsWithoutValidation(string token)
        {
            try
            {
                var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                return jwtToken.Claims;
            }
            catch
            {
                return new List<Claim>();
            }
        }

        // Special Token Generators
        public TokenGenerationResult GeneratePasswordResetToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(TokenClaims.UserId, user.Id.ToString()),
                new Claim(TokenClaims.TokenType, TokenTypes.PasswordReset),
                new Claim(TokenClaims.SecurityStamp, user.SecurityStamp)
            };

            return GenerateJwtToken(claims);
        }

        public TokenGenerationResult GenerateEmailConfirmationToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(TokenClaims.UserId, user.Id.ToString()),
                new Claim(TokenClaims.TokenType, TokenTypes.EmailConfirmation),
                new Claim(TokenClaims.SecurityStamp, user.SecurityStamp)
            };

            return GenerateJwtToken(claims);
        }

        public Shared.DTOs.Auth.TokenValidationResult ValidateRefreshToken(string token)
        {
            // Refresh tokens are validated against the database
            // This method is implemented in the repository
            throw new NotImplementedException("Refresh tokens should be validated against the database");
        }

        public async Task<TokenRevocationResult> RevokeTokenAsync(string jti, string ipAddress)
        {
            // Implementation would mark the token as revoked in the database
            return await Task.FromResult(new TokenRevocationResult
            {
                Success = true,
                TokensRevoked = 1
            });
        }

        public async Task<TokenRevocationResult> RevokeAllTokensForUserAsync(Guid userId)
        {
            // Implementation would revoke all tokens for the user
            return await Task.FromResult(new TokenRevocationResult
            {
                Success = true,
                TokensRevoked = 0 // Actual count would come from database
            });
        }

        public async Task<bool> IsTokenRevokedAsync(string jti)
        {
            // Implementation would check against revoked tokens in database
            return await Task.FromResult(false);
        }

        Shared.DTOs.Auth.TokenValidationResult ITokenService.ValidateToken(string token)
        {
            throw new NotImplementedException();
        }

        Shared.DTOs.Auth.TokenValidationResult ITokenService.ValidateRefreshToken(string token)
        {
            throw new NotImplementedException();
        }

        public TokenGenerationResult GenerateTwoFactorToken(User user, string provider)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Claim> ITokenService.GetUserClaims(User user)
        {
            throw new NotImplementedException();
        }

       
    }
}
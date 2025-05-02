using AuthService.Shared.DTOs.User;
using System;
using System.Security.Claims;

namespace AuthService.Shared.DTOs.Auth
{
    /// <summary>
    /// Unified authentication response for login, registration, and token refresh operations
    /// </summary>
    public class AuthResponse
    {
        /// <summary>
        /// Indicates if the authentication request was successful
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// JWT access token for authenticated requests
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Refresh token for obtaining new access tokens
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// Expiration date/time of the access token
        /// </summary>
        public DateTime TokenExpiration { get; set; }

        /// <summary>
        /// Authenticated user information
        /// </summary>
        public UserDto User { get; set; }

        /// <summary>
        /// Optional message (useful for failures or additional instructions)
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// List of errors (if any occurred during authentication)
        /// </summary>
        public List<string> Errors { get; set; } = new List<string>();

        /// <summary>
        /// Indicates if email verification is required
        /// </summary>
        public bool RequiresEmailVerification { get; set; }

        /// <summary>
        /// Indicates if two-factor authentication is required
        /// </summary>
        public bool RequiresTwoFactor { get; set; }

        /// <summary>
        /// List of available two-factor authentication providers (if required)
        /// </summary>
        public List<string> TwoFactorProviders { get; set; } = new List<string>();
    }

    // Result DTOs
    public class AuthenticationResult : BaseResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTimeOffset TokenExpiration { get; set; }
        public UserDto User { get; set; }
    }

    public class RevocationResult : BaseResponse
    {
        public int TokensRevoked { get; set; }
    }

    public class PasswordResetResult : BaseResponse
    {
        public string ResetToken { get; set; } // For SMS/Email verification
    }

    public class TwoFactorResult : BaseResponse
    {
        public bool TwoFactorEnabled { get; set; }
        public string RecoveryCodes { get; set; } // For first-time setup
    }

    public class VerificationResult : BaseResponse
    {
        public bool Verified { get; set; }
    }

    public class AccountStatusResult : BaseResponse
    {
        public bool IsLocked { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
    } 

    

   

    public class SessionResult : BaseResponse
    {
        public int SessionsTerminated { get; set; }
    }

    public class SessionListResult : BaseResponse
    {
        public IEnumerable<UserSessionDto> Sessions { get; set; }
    }

    public class TokenGenerationResult : BaseResponse
    {
        public string Token { get; set; }
        public DateTimeOffset Expiration { get; set; }
        public string Jti { get; set; } // Unique token identifier
        public IEnumerable<Claim> Claims { get; set; }
    }

    public class TokenValidationResult : BaseResponse
    {
        public bool IsValid { get; set; }
        public bool IsExpired { get; set; }
        public ClaimsPrincipal Principal { get; set; }
        public string FailureReason { get; set; }
        public IEnumerable<Claim> Claims { get; set; }
    }

    public class TokenRevocationResult : BaseResponse
    {
        public int TokensRevoked { get; set; }
        public DateTimeOffset RevocationDate { get; set; } = DateTimeOffset.UtcNow;
    }

    
}
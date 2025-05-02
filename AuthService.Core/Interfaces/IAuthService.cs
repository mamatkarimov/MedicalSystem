using AuthService.Core.Entities;
using AuthService.Shared.DTOs;
using AuthService.Shared.DTOs.Auth;
using AuthService.Shared.DTOs.User;
using System.Threading.Tasks;

namespace AuthService.Core.Interfaces
{
    public interface IAuthService
    {
        // Authentication
        Task<AuthenticationResult> RegisterAsync(RegisterRequest request);
        Task<AuthenticationResult> LoginAsync(LoginRequest request);
        Task<AuthenticationResult> RefreshTokenAsync(TokenRequest request);
        Task<RevocationResult> RevokeTokenAsync(TokenRequest request);
        Task<RevocationResult> RevokeAllRefreshTokensForUserAsync(string userId);

        // Password Management
        Task<PasswordChangeResult> ChangePasswordAsync(string userId, ChangePasswordRequest request);
        Task<PasswordResetResult> RequestPasswordResetAsync(string email);
        Task<PasswordResetResult> ResetPasswordAsync(ResetPasswordRequest request);

        // Two-Factor Authentication
        Task<TwoFactorResult> EnableTwoFactorAsync(string userId);
        Task<TwoFactorResult> DisableTwoFactorAsync(string userId);
        Task<TwoFactorResult> VerifyTwoFactorAsync(TwoFactorVerificationRequest request);

        // Account Verification
        Task<VerificationResult> SendEmailVerificationAsync(string userId);
       // Task<VerificationResult> VerifyEmailAsync(EmailVerificationRequest request);
       // Task<VerificationResult> VerifyPhoneAsync(PhoneVerificationRequest request);

        // Account Status
        Task<AccountStatusResult> LockAccountAsync(string userId, LockAccountRequest request);
        Task<AccountStatusResult> UnlockAccountAsync(string userId);
        Task<AccountStatusResult> DisableAccountAsync(string userId);
        Task<AccountStatusResult> EnableAccountAsync(string userId);

        // Device Management
        Task<DeviceResult> RegisterDeviceAsync(string userId, RegisterDeviceRequest request);
        Task<DeviceResult> RevokeDeviceAsync(string userId, string deviceId);
        Task<DeviceListResult> GetUserDevicesAsync(string userId);

        // Session Management
        Task<SessionListResult> GetActiveSessionsAsync(string userId);
        Task<SessionResult> TerminateSessionAsync(string userId, string sessionId);
        Task<SessionResult> TerminateAllSessionsAsync(string userId);
    }

   
}
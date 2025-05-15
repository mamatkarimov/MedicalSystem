using System.ComponentModel.DataAnnotations;

namespace AuthService.Shared.DTOs.Auth
{
    public class TwoFactorVerificationRequest
    {
        [Required]
        public string Provider { get; set; } // "Email", "Authenticator", "SMS"

        [Required]
        public string Code { get; set; }

        public bool RememberDevice { get; set; }
    }    
}
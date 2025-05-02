using System.ComponentModel.DataAnnotations;

namespace AuthService.Shared.DTOs.Auth
{
    public class TwoFactorRecoveryRequest
    {
        [Required]
        public string RecoveryCode { get; set; }
    }
}
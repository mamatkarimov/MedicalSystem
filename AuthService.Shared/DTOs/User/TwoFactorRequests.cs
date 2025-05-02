using System.ComponentModel.DataAnnotations;

namespace AuthService.Shared.DTOs.User
{
   

    public class TwoFactorRecoveryRequest
    {
        [Required]
        public string RecoveryCode { get; set; }
    }
}

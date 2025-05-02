using System.ComponentModel.DataAnnotations;

namespace AuthService.Shared.DTOs.Auth
{
    public class ResetPasswordRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string ResetToken { get; set; }

        [Required, StringLength(100, MinimumLength = 8)]
        public string NewPassword { get; set; }
    }




}

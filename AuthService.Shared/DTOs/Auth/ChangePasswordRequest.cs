using System.ComponentModel.DataAnnotations;

namespace AuthService.Shared.DTOs.Auth
{
    public class ChangePasswordRequest
    {
        [Required]
        public string CurrentPassword { get; set; }

        [Required, StringLength(100, MinimumLength = 8)]
        public string NewPassword { get; set; }

        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }
    }   
}

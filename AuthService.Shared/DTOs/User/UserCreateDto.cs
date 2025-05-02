using System.ComponentModel.DataAnnotations;

namespace AuthService.Shared.DTOs.User
{
    public class UserCreateDto
    {
        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; }

        [Required, StringLength(100, MinimumLength = 8)]
        public string Password { get; set; }

        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        public string LastName { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        public IEnumerable<string> Roles { get; set; } = new List<string>();
    }
}
using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.Application.Models.Requests
{

    public class RegisterRequest
    {
        [Required]
        public string Username { get; set; } = default!;

        [Required]
        [EmailAddress]
        public string Email { get; set; }        

        [Required]
        public string Password { get; set; } = default!;

        [Required]
        public string Role { get; set; } = default!; // "Doctor", "Patient", etc.       
    }
}
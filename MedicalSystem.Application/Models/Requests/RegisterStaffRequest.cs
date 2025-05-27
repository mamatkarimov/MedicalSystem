namespace MedicalSystem.Application.Models.Requests
{
    public class RegisterStaffRequest
    {
        public string Username { get; set; } = default!; 
        public string Password { get; set; } = default!;
        public string Role { get; set; } = "Doctor"; // e.g., "Doctor", "Nurse"
        public string Position { get; set; } = default!;
        public string? Department { get; set; } // optional
        public string Email { get; set; } = default!;

    }
}
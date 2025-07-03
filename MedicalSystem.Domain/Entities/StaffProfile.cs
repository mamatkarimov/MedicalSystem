using System;
namespace MedicalSystem.Domain.Entities
{
    public class StaffProfile
    {
        public int Id { get; set; }
        public Guid? UserId { get; set; }
        public User? User { get; set; }
        public string Position { get; set; } // Example: "Doctor", "Nurse", "Admin", etc.
        public string? Department { get; set; } // Optional: e.g., "Cardiology"
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
    }

}
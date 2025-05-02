using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace MedicalSystem.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        // Link to Identity user (AuthService)
        public string IdentityId { get; set; } = null!;  // Stores AuthService's ApplicationUser.Id

        // User profile data
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? ProfessionalTitle { get; set; }
        public DateTime? DateOfBirth { get; set; }

        // Domain relationships
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        //public ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();

        // Audit fields
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
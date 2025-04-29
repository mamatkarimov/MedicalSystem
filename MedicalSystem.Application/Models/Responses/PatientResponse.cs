using System;
using System.Collections.Generic;

namespace MedicalSystem.Application.Models.Responses
{
    public class PatientResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string InsuranceNumber { get; set; }

        // For related data
        public List<AppointmentResponse> Appointments { get; set; } = new();

        // Computed property
        public string FullName => $"{FirstName} {LastName}";
    }
}

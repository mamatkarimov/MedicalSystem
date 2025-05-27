using System;

namespace MedicalSystem.Application.Models.Responses
{
    public class PatientListItem
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
    }
}

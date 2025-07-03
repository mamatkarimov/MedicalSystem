using System;
using System.Collections.Generic;
namespace MedicalSystem.Domain.Entities
{
    public class Patient
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public User? User { get; set; }
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } = default!;

        public bool IsActive { get; set; } = true;

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<HospitalVisit> HospitalVisits { get; set; } = new List<HospitalVisit>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
    }

}
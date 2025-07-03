using System;
namespace MedicalSystem.Domain.Entities
{
    public class MedicalRecord
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; } = default!;
        public Guid DoctorId { get; set; }
        public User Doctor { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public string Anamnesis { get; set; } = default!;
        public string Diagnosis { get; set; } = default!;
        public string Prescriptions { get; set; } = default!;
    }

}
using System;
namespace MedicalSystem.Domain.Entities
{
    public class HospitalVisit
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; } = default!;
        public DateTime AdmissionDate { get; set; }
        public DateTime? DischargeDate { get; set; }
        public string BedNumber { get; set; } = default!;
        public string Notes { get; set; } = default!;
    }

}
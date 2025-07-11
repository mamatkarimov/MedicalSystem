using MedicalSystem.Domain.Entities1;
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
        public ICollection<LabOrder> LabOrders { get; set; } = new List<LabOrder>();
        public ICollection<PatientDiet> PatientDiets { get; set; } = new List<PatientDiet>();
        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
        public ICollection<QueueItem> QueueItems { get; set; } = new List<QueueItem>();
        public ICollection<PatientQueue> PatientQueues { get; set; } = new List<PatientQueue>();
        public ICollection<MedicalHistory> MedicalHistories { get; set; } = new List<MedicalHistory>();
        public ICollection<NurseRound> NurseRounds { get; set; } = new List<NurseRound>();
        public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
        public ICollection<Hospitalization> Hospitalizations { get; set; } = new List<Hospitalization>();
        
    }

}
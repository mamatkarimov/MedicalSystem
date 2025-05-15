using System;
namespace MedicalSystem.Domain.Entities
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }
        public DateTime Date { get; set; }
        public string Symptoms { get; set; } = "";
        public string Status { get; set; } = "Pending";

        public User? Patient { get; set; }
        public User? Doctor { get; set; }
    }
}
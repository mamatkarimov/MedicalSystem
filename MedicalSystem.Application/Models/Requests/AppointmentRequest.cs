using System;

namespace MedicalSystem.Application.Models.Requests
{
    public class AppointmentRequest
    {
        public Guid DoctorId { get; set; }
        public DateTime Date { get; set; }
        public string Symptoms { get; set; } = string.Empty;
    }
}
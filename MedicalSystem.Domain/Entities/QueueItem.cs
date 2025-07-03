using System;
namespace MedicalSystem.Domain.Entities
{
    public class QueueItem
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public string Department { get; set; } = default!;
        public string Status { get; set; } = "Waiting";
    }

}
using System.ComponentModel.DataAnnotations;

namespace MedicalSystemAPI.Models.DTOs
{
    public class PatientQueueDto
    {
        public int QueueID { get; set; }
        public int PatientID { get; set; }
        public int? AppointmentID { get; set; }
        public DateTime QueueDate { get; set; }
        public string Status { get; set; } = "Waiting";
        public int Priority { get; set; } = 5;
        public int? DepartmentID { get; set; }
        public string Notes { get; set; }
    }

    public class AddToQueueRequest
    {
        [Required]
        public int PatientID { get; set; }
        
        public int? AppointmentID { get; set; }
        public int Priority { get; set; } = 5;
        public int? DepartmentID { get; set; }
        public string Notes { get; set; }
    }

    public class QueueStatusUpdateRequest
    {
        [Required]
        public string Status { get; set; } // InProgress, Completed, Cancelled
    }
}
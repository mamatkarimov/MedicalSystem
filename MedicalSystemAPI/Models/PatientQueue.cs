using System.ComponentModel.DataAnnotations;
using MedicalSystemAPI.Models.DTOs;

namespace MedicalSystemAPI.Models
{
    public class PatientQueue
    {
        public int QueueID { get; set; }
        
        [Required]
        public int PatientID { get; set; }
        
        public int? AppointmentID { get; set; }
        
        public DateTime QueueDate { get; set; } = DateTime.UtcNow;
        
        [Required]
        public string Status { get; set; } = "Waiting"; // Waiting, InProgress, Completed, Cancelled
        
        [Required]
        public int Priority { get; set; } = 5; // 1 highest, 10 lowest
        
        public int? DepartmentID { get; set; }
        public string Notes { get; set; }
        
        // Navigation properties
        public Patient Patient { get; set; }
        public Appointment Appointment { get; set; }
        public Department Department { get; set; }
    }
}
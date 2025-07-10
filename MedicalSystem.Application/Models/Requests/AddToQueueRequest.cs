using System;
using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.Application.Models.Requests
{
    public class AddToQueueRequest
    {
        [Required]
        public Guid PatientID { get; set; }
        
        public int? AppointmentID { get; set; }
        public int Priority { get; set; } = 5;
        public int? DepartmentID { get; set; }
        public string Notes { get; set; }
    }
}
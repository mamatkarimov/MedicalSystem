using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.API.Models.Requests
{
    public class AddToQueueRequest
    {
        [Required]
        public int PatientID { get; set; }
        
        public int? AppointmentID { get; set; }
        public int Priority { get; set; } = 5;
        public int? DepartmentID { get; set; }
        public string Notes { get; set; }
    }
}
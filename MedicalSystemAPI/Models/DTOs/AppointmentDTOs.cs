using System.ComponentModel.DataAnnotations;

namespace MedicalSystemAPI.Models.DTOs
{
    public class Appointment
{
    [Key]
        public int AppointmentID { get; set; }
    
    [Required]
    public int PatientID { get; set; }
    
    [Required]
    public string DoctorID { get; set; }
    
    [Required]
    public DateTime AppointmentDate { get; set; }
    
    [Required]
    public string Status { get; set; } // Scheduled, Completed, Cancelled, NoShow
    
    public string Reason { get; set; }
    public string Diagnosis { get; set; }
    public string TreatmentPlan { get; set; }
    public string Notes { get; set; }
    
    // Navigation properties
    public Patient Patient { get; set; }
    public ApplicationUser Doctor { get; set; }
    public ICollection<MedicalHistory> MedicalHistories { get; set; }    
}

public class CreateAppointmentRequest
{
    [Required]
    public int PatientID { get; set; }
    
    [Required]
    public string DoctorID { get; set; }
    
    [Required]
    public DateTime AppointmentDate { get; set; }
    
    public string Reason { get; set; }

}
}
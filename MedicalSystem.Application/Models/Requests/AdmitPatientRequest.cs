using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.Application.Models.Requests
{
    public class AdmitPatientRequest
{
    [Required]
    public int PatientID { get; set; }
    
    [Required]
    public int BedID { get; set; }
    
    [Required]
    public string DiagnosisOnAdmission { get; set; }
    
    [Required]
    public string AttendingDoctorID { get; set; }
}
}
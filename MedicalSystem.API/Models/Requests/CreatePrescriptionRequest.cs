using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.API.Models.Requests
{
    public class CreatePrescriptionRequest
{
    [Required]
    public int PatientID { get; set; }
    
    [Required]
    public string Medication { get; set; }
    
    public string Dosage { get; set; }
    public string Frequency { get; set; }
    public string Duration { get; set; }
    public string Instructions { get; set; }
}
}
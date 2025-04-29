using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.API.Models.Requests
{
    public class AddMedicalHistoryRequest
{
    [Required]
    public int PatientID { get; set; }
    
    [Required]
    public string HistoryType { get; set; }
    
    [Required]
    public string Description { get; set; }
}
}
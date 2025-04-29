using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.API.Models.Requests
{
    public class CreateLabOrderRequest
{
    [Required]
    public int PatientID { get; set; }
    
    [Required]
    public List<int> TestTypeIDs { get; set; }
    
    public string Priority { get; set; } = "Routine";
    public string Notes { get; set; }
}
}
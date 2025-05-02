using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.Application.Models.Requests
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
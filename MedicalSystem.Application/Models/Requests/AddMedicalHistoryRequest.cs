using System;
using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.Application.Models.Requests
{
    public class AddMedicalHistoryRequest
{
    [Required]
    public Guid PatientID { get; set; }
    
    [Required]
    public string HistoryType { get; set; }
    
    [Required]
    public string Description { get; set; }
}
}
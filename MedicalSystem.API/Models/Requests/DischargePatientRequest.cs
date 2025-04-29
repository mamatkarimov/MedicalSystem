using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.API.Models.Requests
{
    public class DischargePatientRequest
{
    [Required]
    public string DiagnosisOnDischarge { get; set; }
}
}
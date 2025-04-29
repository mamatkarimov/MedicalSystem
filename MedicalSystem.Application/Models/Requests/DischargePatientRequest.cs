using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.Application.Models.Requests
{
    public class DischargePatientRequest
{
    [Required]
    public string DiagnosisOnDischarge { get; set; }
}
}
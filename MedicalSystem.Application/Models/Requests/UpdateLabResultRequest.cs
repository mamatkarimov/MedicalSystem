using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.Application.Models.Requests
{
    public class UpdateLabResultRequest
{
    [Required]
    public string Result { get; set; }
    public string ReferenceRange { get; set; }
}
}
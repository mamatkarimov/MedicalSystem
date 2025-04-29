using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.API.Models.Requests
{
    public class UpdateLabResultRequest
{
    [Required]
    public string Result { get; set; }
    public string ReferenceRange { get; set; }
}
}
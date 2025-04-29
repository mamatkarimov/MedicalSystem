using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.Application.Models.Requests
{
    public class UpdateInstrumentalStudyResultRequest
    {
        [Required]
        public string Results { get; set; }
        
        [Required]
        public string Conclusion { get; set; }
    }
}
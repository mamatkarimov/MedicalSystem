using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.API.Models.Requests
{
    public class UpdateInstrumentalStudyResultRequest
    {
        [Required]
        public string Results { get; set; }
        
        [Required]
        public string Conclusion { get; set; }
    }
}
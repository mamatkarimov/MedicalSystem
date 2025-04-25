using System.ComponentModel.DataAnnotations;

namespace MedicalSystemAPI.Models.DTOs
{
    public class InstrumentalStudyDto
    {
        public int StudyID { get; set; }
        public int PatientID { get; set; }
        public string StudyType { get; set; } // УЗИ, ЭКГ, рентген и т.д.
        public string OrderedByID { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? PerformedDate { get; set; }
        public string PerformedByID { get; set; }
        public string Results { get; set; }
        public string Conclusion { get; set; }
        public string Status { get; set; } = "Pending";
    }

    public class CreateInstrumentalStudyRequest
    {
        [Required]
        public int PatientID { get; set; }
        
        [Required]
        public string StudyType { get; set; }
        
        public string Notes { get; set; }
    }

    public class UpdateInstrumentalStudyResultRequest
    {
        [Required]
        public string Results { get; set; }
        
        [Required]
        public string Conclusion { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.Domain.Entities
{
    public class InstrumentalStudy
    {
[Key]
        public int StudyID { get; set; }
        [Required]
        public int PatientID { get; set; }
        [Required]
        public string StudyType { get; set; } // УЗИ, ЭКГ, рентген и т.д.
        [Required]
        public string OrderedByID { get; set; }
        public DateTime OrderDate { get; set; }= DateTime.UtcNow;
        public DateTime? PerformedDate { get; set; }
        public string PerformedByID { get; set; }
        public string Results { get; set; }
        public string Conclusion { get; set; }
        public string Status { get; set; } = "Pending";
        
        // Navigation properties
        public Patient Patient { get; set; }
        public ApplicationUser OrderedBy { get; set; }
        public ApplicationUser PerformedBy { get; set; }
    }

   

   
}
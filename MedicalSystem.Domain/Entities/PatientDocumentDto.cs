using System.ComponentModel.DataAnnotations;
namespace MedicalSystem.Domain.Entities
{
    public class PatientDocument
    {
        [Key]
        public int DocumentID { get; set; }
        [Required]
        public int PatientID { get; set; }
        
        [Required]
        public string DocumentType { get; set; } // Паспорт, Мед. страховка и т.д.
        
        [Required]
        public string DocumentNumber { get; set; }
        
        public DateTime? IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string IssuedBy { get; set; }

        public Patient Patient { get; set; }
    }
   
    

    
}
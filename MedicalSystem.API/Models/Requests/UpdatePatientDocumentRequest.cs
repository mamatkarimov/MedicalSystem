using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.API.Models.Requests
{
    public class UpdatePatientDocumentRequest
    {
        [Required]
        public string DocumentType { get; set; }
        
        [Required]
        public string DocumentNumber { get; set; }
        
        public DateTime? IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string IssuedBy { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.Application.Models.Requests
{
    public class AddPatientDocumentRequest
    {
        [Required]
        public Guid PatientID { get; set; }
        
        [Required]
        public string DocumentType { get; set; }
        
        [Required]
        public string DocumentNumber { get; set; }
        
        public DateTime? IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string IssuedBy { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.Application.Models.Requests
{
    public class CreateInstrumentalStudyRequest
    {
        [Required]
        public Guid PatientID { get; set; }
        
        [Required]
        public string StudyType { get; set; }
        
        public string Notes { get; set; }
    }
}
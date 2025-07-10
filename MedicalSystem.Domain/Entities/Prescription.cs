using System;
using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.Domain.Entities
{
    public class Prescription
{

[Key]
    public int PrescriptionID { get; set; }
    
    [Required]
    public Guid PatientID { get; set; }
    
    [Required]
    public Guid PrescribedByID { get; set; }
    
    public DateTime PrescriptionDate { get; set; } = DateTime.UtcNow;
    
    [Required]
    public string Medication { get; set; }
    
    public string Dosage { get; set; }
    public string Frequency { get; set; }
    public string Duration { get; set; }
    public string Instructions { get; set; }
    
    [Required]
    public string Status { get; set; } = "Active";
    
    // Navigation properties
    public Patient Patient { get; set; }
    public User PrescribedBy { get; set; }
}




}
using System;
using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.Domain.Entities
{
    public class PatientDiet
{

[Key]    public int DietID { get; set; }
    
    [Required]
    public int PatientID { get; set; }
    
    [Required]
    public int HospitalizationID { get; set; }
    
    [Required]
    public string DietType { get; set; }
    
    [Required]
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    
    public DateTime? EndDate { get; set; }
    public string Notes { get; set; }
    
    // Navigation properties
    public Patient Patient { get; set; }
    public Hospitalization Hospitalization { get; set; }
}


}
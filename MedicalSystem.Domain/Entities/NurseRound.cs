using System;
using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.Domain.Entities
{
    public class NurseRound
{

[Key]
    public int RoundID { get; set; }
    
    [Required]
    public Guid NurseID { get; set; }
    
    [Required]
    public Guid PatientID { get; set; }
    
    public DateTime RoundDate { get; set; } = DateTime.UtcNow;
    public decimal? Temperature { get; set; }
    public string BloodPressure { get; set; }
    public int? Pulse { get; set; }
    public int? RespirationRate { get; set; }
    public string Notes { get; set; }
    
    // Navigation properties
    public User Nurse { get; set; }
    public Patient Patient { get; set; }
}


}
using System;
using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.Domain.Entities
{
public class MedicalHistory
{
    [Key]
    public int HistoryID { get; set; }
    
    [Required]
    public int PatientID { get; set; }
    public int? AppointmentID { get; set; }
    
    public DateTime RecordDate { get; set; } = DateTime.UtcNow;
    
    [Required]
    public string RecordedByID { get; set; }
    
    [Required]
    public string HistoryType { get; set; } // Anamnesis, Allergy, Chronic Disease, etc.
    
    public string Description { get; set; }
    
    // Navigation properties
    public Patient Patient { get; set; }
    public Appointment Appointment { get; set; }  // Добавляем навигационное свойство
    public User RecordedBy { get; set; }
}




}
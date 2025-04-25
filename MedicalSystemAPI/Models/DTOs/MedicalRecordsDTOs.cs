using System.ComponentModel.DataAnnotations;

namespace MedicalSystemAPI.Models.DTOs
{
public class MedicalHistory
{
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
    public ApplicationUser RecordedBy { get; set; }
}

public class Prescription
{
    public int PrescriptionID { get; set; }
    
    [Required]
    public int PatientID { get; set; }
    
    [Required]
    public string PrescribedByID { get; set; }
    
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
    public ApplicationUser PrescribedBy { get; set; }
}

public class AddMedicalHistoryRequest
{
    [Required]
    public int PatientID { get; set; }
    
    [Required]
    public string HistoryType { get; set; }
    
    [Required]
    public string Description { get; set; }
}

public class CreatePrescriptionRequest
{
    [Required]
    public int PatientID { get; set; }
    
    [Required]
    public string Medication { get; set; }
    
    public string Dosage { get; set; }
    public string Frequency { get; set; }
    public string Duration { get; set; }
    public string Instructions { get; set; }
}
}
using System.ComponentModel.DataAnnotations;

namespace MedicalSystemAPI.Models.DTOs
{
public class Department
{
    public int DepartmentID { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    public string Description { get; set; }
    public string HeadDoctorID { get; set; }
    
    // Navigation properties
    public ApplicationUser HeadDoctor { get; set; }
    public ICollection<Ward> Wards { get; set; }
}

public class Ward
{
    public int WardID { get; set; }
    
    [Required]
    public int DepartmentID { get; set; }
    
    [Required]
    public string WardNumber { get; set; }
    
    [Required]
    public int Capacity { get; set; }
    
    [Required]
    public char GenderSpecific { get; set; } // 'M', 'F', or 'N'
    
    // Navigation properties
    public Department Department { get; set; }
    public ICollection<Bed> Beds { get; set; }
}

public class Bed
{
    public int BedID { get; set; }
    
    [Required]
    public int WardID { get; set; }
    
    [Required]
    public string BedNumber { get; set; }
    
    [Required]
    public bool IsOccupied { get; set; } = false;
    
    // Navigation properties
    public Ward Ward { get; set; }
    public ICollection<Hospitalization> Hospitalizations { get; set; }
}

public class Hospitalization
{
    public int HospitalizationID { get; set; }
    
    [Required]
    public int PatientID { get; set; }
    
    [Required]
    public int BedID { get; set; }
    
    [Required]
    public DateTime AdmissionDate { get; set; }
    
    public DateTime? DischargeDate { get; set; }
    public string DiagnosisOnAdmission { get; set; }
    public string DiagnosisOnDischarge { get; set; }
    public string AttendingDoctorID { get; set; }
    
    [Required]
    public string Status { get; set; } // Active, Discharged, Transferred
    
    // Navigation properties
    public Patient Patient { get; set; }
    public Bed Bed { get; set; }
    public ApplicationUser AttendingDoctor { get; set; }
    public ICollection<NurseRound> NurseRounds { get; set; }
    public ICollection<PatientDiet> PatientDiets { get; set; }
}

public class NurseRound
{
    public int RoundID { get; set; }
    
    [Required]
    public string NurseID { get; set; }
    
    [Required]
    public int PatientID { get; set; }
    
    public DateTime RoundDate { get; set; } = DateTime.UtcNow;
    public decimal? Temperature { get; set; }
    public string BloodPressure { get; set; }
    public int? Pulse { get; set; }
    public int? RespirationRate { get; set; }
    public string Notes { get; set; }
    
    // Navigation properties
    public ApplicationUser Nurse { get; set; }
    public Patient Patient { get; set; }
}

public class PatientDiet
{
    public int DietID { get; set; }
    
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

public class AdmitPatientRequest
{
    [Required]
    public int PatientID { get; set; }
    
    [Required]
    public int BedID { get; set; }
    
    [Required]
    public string DiagnosisOnAdmission { get; set; }
    
    [Required]
    public string AttendingDoctorID { get; set; }
}

public class DischargePatientRequest
{
    [Required]
    public string DiagnosisOnDischarge { get; set; }
}

}
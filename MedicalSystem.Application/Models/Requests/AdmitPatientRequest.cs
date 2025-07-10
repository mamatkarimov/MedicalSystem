using System;
using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.Application.Models.Requests
{
    public class AdmitPatientRequest
{
    [Required]
    public Guid PatientID { get; set; }
    
    [Required]
    public int BedID { get; set; }
    
    [Required]
    public string DiagnosisOnAdmission { get; set; }
    
    [Required]
    public Guid AttendingDoctorID { get; set; }
}
}
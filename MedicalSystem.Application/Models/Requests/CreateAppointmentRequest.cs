using System;
using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.Application.Models.Requests
{
    public class CreateAppointmentRequest
{
    [Required]
    public int PatientID { get; set; }
    
    [Required]
    public string DoctorID { get; set; }
    
    [Required]
    public DateTime AppointmentDate { get; set; }
    
    public string Reason { get; set; }

}
}
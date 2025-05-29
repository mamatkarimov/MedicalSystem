using System;
using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.Application.Models.Requests
{
    public class CreateAppointmentRequest
{
    [Required]
    public Guid PatientID { get; set; }
    
    [Required]
    public Guid DoctorID { get; set; }
    
    [Required]
    public DateTime AppointmentDate { get; set; }
    
    public string Symptoms { get; set; }

}
}
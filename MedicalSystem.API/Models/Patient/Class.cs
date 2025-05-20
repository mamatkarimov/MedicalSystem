using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.API.Models.Patient
{
    public class PatientRegistrationDto
    {
        [Required] public string Username { get; set; }
        [Required] public string Password { get; set; }
        [Required] public string Email { get; set; }
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        [Required] public DateTime DateOfBirth { get; set; }
        [Required] public string Gender { get; set; }
        public string? PhoneNumber { get; set; }
    }

    public class AssignServiceDto
    {
        [Required] public Guid PatientId { get; set; }
        [Required] public Guid ServiceId { get; set; }
        [Required] public Guid DoctorId { get; set; }
        public string? Diagnosis { get; set; }
        public string? Prescriptions { get; set; }
        public DateTime? AppointmentDate { get; set; }
    }

    public class PatientServiceResultDto
    {
        public Guid Id { get; set; }
        public string ServiceName { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public DateTime AssignedDate { get; set; }
        public string DoctorName { get; set; }
        public string? Diagnosis { get; set; }
    }
}

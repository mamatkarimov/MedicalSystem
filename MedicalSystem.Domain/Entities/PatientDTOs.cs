using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.Domain.Entities
{
   public class Patient
{
    [Key]
        public int PatientID { get; set; }
    
    [Required]
    public string FirstName { get; set; }
    
    [Required]
    public string LastName { get; set; }
    
    public string MiddleName { get; set; }
    
    [Required]
    public DateTime BirthDate { get; set; }
    
    [Required]
    public char Gender { get; set; } // 'M' or 'F'
    
    public string Address { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string InsuranceNumber { get; set; }
    public string InsuranceCompany { get; set; }
    
    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
     public ICollection<PatientDocument> PatientDocuments { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<Hospitalization> Hospitalizations { get; set; }
}


}
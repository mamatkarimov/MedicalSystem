using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.API.Models.Requests
{
    public class RegisterPatientRequest
{
    [Required]
    public string FirstName { get; set; }
    
    [Required]
    public string LastName { get; set; }
    
    public string MiddleName { get; set; }
    
    [Required]
    public DateTime BirthDate { get; set; }
    
    [Required]
    public char Gender { get; set; }
    
    public string Address { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string InsuranceNumber { get; set; }
    public string InsuranceCompany { get; set; }
    
    // For self-registration
    public string Username { get; set; }
    public string Password { get; set; }
} 
}
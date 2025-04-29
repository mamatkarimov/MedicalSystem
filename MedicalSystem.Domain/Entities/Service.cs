using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.Domain.Entities
{
    
    public class Service
{
    [Key]
    public int ServiceID { get; set; }
    
    [Required]
    public string ServiceName { get; set; }
    
    public string Description { get; set; }
    public string Category { get; set; } // Consultation, LabTest, InstrumentalStudy, etc.
    
    [Required]
    public decimal Price { get; set; }
    
    [Required]
    public bool IsActive { get; set; } = true;
}


}
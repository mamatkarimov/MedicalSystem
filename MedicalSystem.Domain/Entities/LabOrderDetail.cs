using System;
using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.Domain.Entities
{
    public class LabOrderDetail
{

[Key] 
    public int Id { get; set; }
    
    [Required]
    public int OrderId { get; set; }
    
    [Required]
    public int TestTypeId { get; set; }
    
    [Required]
    public string Status { get; set; } = "Pending";
    
    public string Result { get; set; }
    public DateTime? ResultDate { get; set; }
    public Guid PerformedById { get; set; }
    public string ReferenceRange { get; set; }
    
    // Navigation properties
    public LabOrder LabOrder { get; set; }
    public LabTestType TestType { get; set; }
    public User PerformedBy { get; set; }
}




}
using System;
using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.Domain.Entities
{
    public class LabOrderDetail
{

[Key] 
    public int OrderDetailID { get; set; }
    
    [Required]
    public int OrderID { get; set; }
    
    [Required]
    public int TestTypeID { get; set; }
    
    [Required]
    public string Status { get; set; } = "Pending";
    
    public string Result { get; set; }
    public DateTime? ResultDate { get; set; }
    public string PerformedByID { get; set; }
    public string ReferenceRange { get; set; }
    
    // Navigation properties
    public LabOrder LabOrder { get; set; }
    public LabTestType TestType { get; set; }
    public ApplicationUser PerformedBy { get; set; }
}




}
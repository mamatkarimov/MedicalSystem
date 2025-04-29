using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.Domain.Entities
{
    public class LabOrder
{
    [Key] 
    public int OrderID { get; set; }
    
    [Required]
    public int PatientID { get; set; }
    
    [Required]
    public string OrderedByID { get; set; }
    
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    
    [Required]
    public string Status { get; set; } = "Pending"; // Pending, InProgress, Completed, Cancelled
    
    [Required]
    public string Priority { get; set; } = "Routine"; // Routine, Urgent, STAT
    
    public string Notes { get; set; }
    
    // Navigation properties
    public Patient Patient { get; set; }
    public ApplicationUser OrderedBy { get; set; }
    public ICollection<LabOrderDetail> LabOrderDetails { get; set; }
}




}
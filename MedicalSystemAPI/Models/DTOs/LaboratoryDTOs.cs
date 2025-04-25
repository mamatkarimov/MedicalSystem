using System.ComponentModel.DataAnnotations;

namespace MedicalSystemAPI.Models.DTOs
{
    public class LabTestType
{
    public int TestTypeID { get; set; }
    
    [Required]
    public string TestName { get; set; }
    
    public string Description { get; set; }
    public string SampleType { get; set; }
    public string PreparationInstructions { get; set; }
    public string NormalRange { get; set; }
    
    // Navigation properties
    public ICollection<LabOrderDetail> LabOrderDetails { get; set; }
}

public class LabOrder
{
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

public class LabOrderDetail
{
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

public class CreateLabOrderRequest
{
    [Required]
    public int PatientID { get; set; }
    
    [Required]
    public List<int> TestTypeIDs { get; set; }
    
    public string Priority { get; set; } = "Routine";
    public string Notes { get; set; }
}

public class UpdateLabResultRequest
{
    [Required]
    public string Result { get; set; }
    public string ReferenceRange { get; set; }
}
}
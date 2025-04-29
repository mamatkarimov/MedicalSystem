using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.Domain.Entities
{
    public class LabTestType
{
    [Key] 
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




}
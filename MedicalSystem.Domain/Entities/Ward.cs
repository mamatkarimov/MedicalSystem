using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.Domain.Entities
{
    public class Ward
{
    [Key]
    public int WardID { get; set; }
    
    [Required]
    public int DepartmentID { get; set; }
    
    [Required]
    public string WardNumber { get; set; }
    
    [Required]
    public int Capacity { get; set; }
    
    [Required]
    public char GenderSpecific { get; set; } // 'M', 'F', or 'N'
    
    // Navigation properties
    public Department Department { get; set; }
    public ICollection<Bed> Beds { get; set; }
}




}
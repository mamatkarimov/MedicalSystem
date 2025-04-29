using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.Domain.Entities
{
    public class Bed
{

[Key]
    public int BedID { get; set; }
    
    [Required]
    public int WardID { get; set; }
    
    [Required]
    public string BedNumber { get; set; }
    
    [Required]
    public bool IsOccupied { get; set; } = false;
    
    // Navigation properties
    public Ward Ward { get; set; }
    public ICollection<Hospitalization> Hospitalizations { get; set; }
}


}
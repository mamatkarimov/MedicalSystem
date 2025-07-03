using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.Domain.Entities
{
    public class InvoiceItem
{
    [Required]
    public int ServiceID { get; set; }
    
    [Required]
    public int Quantity { get; set; } = 1;
    
    public decimal Discount { get; set; } = 0;
}


}
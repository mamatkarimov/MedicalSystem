using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.Domain.Entities
{
    public class InvoiceDetail
{
    [Key]
    public int InvoiceDetailID { get; set; }
    
    [Required]
    public int InvoiceID { get; set; }
    
    [Required]
    public int ServiceID { get; set; }
    
    [Required]
    public int Quantity { get; set; } = 1;
    
    [Required]
    public decimal UnitPrice { get; set; }
    
    [Required]
    public decimal Discount { get; set; } = 0;
    
    // Navigation properties
    public Invoice Invoice { get; set; }
    public Service Service { get; set; }
}


}
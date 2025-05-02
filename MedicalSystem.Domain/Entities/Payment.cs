using System;
using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.Domain.Entities
{
    public class Payment
{
    [Key]
    public int PaymentID { get; set; }
    
    [Required]
    public int InvoiceID { get; set; }
    
    [Required]
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    
    [Required]
    public decimal Amount { get; set; }
    
    [Required]
    public string PaymentMethod { get; set; } // Cash, Card, Insurance, etc.
    
    [Required]
    public string ReceivedByID { get; set; }
    
    public string Notes { get; set; }
    
    // Navigation properties
    public Invoice Invoice { get; set; }
    public User ReceivedBy { get; set; }
}


}
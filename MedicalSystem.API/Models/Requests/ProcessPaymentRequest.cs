using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.API.Models.Requests
{
    public class ProcessPaymentRequest
{
    [Required]
    public int InvoiceID { get; set; }
    
    [Required]
    public decimal Amount { get; set; }
    
    [Required]
    public string PaymentMethod { get; set; }
    
    public string Notes { get; set; }
}
}
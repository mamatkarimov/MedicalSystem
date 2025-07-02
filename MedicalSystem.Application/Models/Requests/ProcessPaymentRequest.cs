using System;
using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.Application.Models.Requests
{
    public class ProcessPaymentRequest
{
    [Required]
    public Guid InvoiceId { get; set; }
    
    [Required]
    public decimal Amount { get; set; }
    
    [Required]
    public string PaymentMethod { get; set; }
    
    public string Notes { get; set; }
}
}
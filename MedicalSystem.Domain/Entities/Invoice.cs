using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.Domain.Entities
{
    public class Invoice
{
    [Key]
    public Guid InvoiceID { get; set; }
    
    [Required]
    public Guid PatientID { get; set; }
    
    [Required]
    public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;
    
    public DateTime? DueDate { get; set; }
    
    [Required]
    public decimal TotalAmount { get; set; }
    
    [Required]
    public decimal PaidAmount { get; set; } = 0;
    
    [Required]
    public string Status { get; set; } = "Pending"; // Pending, PartiallyPaid, Paid, Cancelled
    
    [Required]
    public Guid CreatedByID { get; set; }
    
    // Navigation properties
    public Patient Patient { get; set; }
    public User CreatedBy { get; set; }
    public ICollection<InvoiceDetail> InvoiceDetails { get; set; }
    public ICollection<Payment> Payments { get; set; }
}


}
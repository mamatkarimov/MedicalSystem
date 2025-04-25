using System.ComponentModel.DataAnnotations;

namespace MedicalSystemAPI.Models.DTOs
{
    public class Service
{
    public int ServiceID { get; set; }
    
    [Required]
    public string ServiceName { get; set; }
    
    public string Description { get; set; }
    public string Category { get; set; } // Consultation, LabTest, InstrumentalStudy, etc.
    
    [Required]
    public decimal Price { get; set; }
    
    [Required]
    public bool IsActive { get; set; } = true;
}

public class Invoice
{
    public int InvoiceID { get; set; }
    
    [Required]
    public int PatientID { get; set; }
    
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
    public string CreatedByID { get; set; }
    
    // Navigation properties
    public Patient Patient { get; set; }
    public ApplicationUser CreatedBy { get; set; }
    public ICollection<InvoiceDetail> InvoiceDetails { get; set; }
    public ICollection<Payment> Payments { get; set; }
}

public class InvoiceDetail
{
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

public class Payment
{
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
    public ApplicationUser ReceivedBy { get; set; }
}

public class CreateInvoiceRequest
{
    [Required]
    public int PatientID { get; set; }
    
    public List<InvoiceItem> Items { get; set; }
    
    public string PaymentMethod { get; set; }
}

public class InvoiceItem
{
    [Required]
    public int ServiceID { get; set; }
    
    [Required]
    public int Quantity { get; set; } = 1;
    
    public decimal Discount { get; set; } = 0;
}

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
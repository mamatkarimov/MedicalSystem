using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.Application.Models.Requests
{
    public class CreateInvoiceRequest
{
    [Required]
    public int PatientID { get; set; }
    
    public List<InvoiceItem> Items { get; set; }
    
    public string PaymentMethod { get; set; }
}
}
using System;
using System.Collections.Generic;
namespace MedicalSystem.Domain.Entities
{
    public class Payment
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; } = default!;
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = default!;
        public string? ReceiptNumber { get; set; }
        public string ReceivedByID { get; set; }
        public ICollection<Refund> Refunds { get; set; } = new List<Refund>();
        public string Notes { get; set; } = default!;
        public Guid InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
        public User ReceivedBy { get; set; }
    }

}
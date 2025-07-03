using System;
namespace MedicalSystem.Domain.Entities
{
    public class Refund
    {
        public Guid Id { get; set; }
        public Guid PaymentId { get; set; }
        public Payment Payment { get; set; } = default!;
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; } = default!;
    }

}
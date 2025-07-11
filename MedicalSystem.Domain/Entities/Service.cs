using MedicalSystem.Domain.Entities1;
using System;
using System.Collections.Generic;
namespace MedicalSystem.Domain.Entities
{
    public class Service
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }
        public string Category { get; set; } = default!;
        public bool IsActive { get; set; } = true;

        public ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();
    }

}
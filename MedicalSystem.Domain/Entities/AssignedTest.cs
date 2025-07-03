using System;
using System.Collections.Generic;
namespace MedicalSystem.Domain.Entities
{
    public class AssignedTest
    {
        public Guid Id { get; set; }
        public Guid AppointmentId { get; set; }
        public Appointment Appointment { get; set; } = default!;
        public Guid TestTemplateId { get; set; }
        public TestTemplate TestTemplate { get; set; } = default!;
        public ICollection<TestResult> Results { get; set; } = new List<TestResult>();
    }

}
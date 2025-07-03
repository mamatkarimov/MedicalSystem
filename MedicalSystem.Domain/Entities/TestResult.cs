using System;
namespace MedicalSystem.Domain.Entities
{
    public class TestResult
    {
        public Guid Id { get; set; }
        public Guid AssignedTestId { get; set; }
        public AssignedTest AssignedTest { get; set; } = default!;
        public string ParameterName { get; set; } = default!;
        public string Value { get; set; } = default!;
        public string Unit { get; set; } = default!;
        public string ReferenceRange { get; set; } = default!;
    }

}
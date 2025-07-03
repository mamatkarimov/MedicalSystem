using System;
namespace MedicalSystem.Domain.Entities
{
    public class TestTemplate
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        //public ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
    }

}
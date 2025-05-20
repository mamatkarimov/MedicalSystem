using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace MedicalSystem.Domain.Entities
{    
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Username { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public bool IsActive { get; set; } = true;
        public string Role { get; set; } = default!;
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }

    public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }

    public class UserRole
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = default!;
        public Guid RoleId { get; set; }
        public Role Role { get; set; } = default!;
    }

    public class Patient
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public User? User { get; set; }
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } = default!;

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<HospitalVisit> HospitalVisits { get; set; } = new List<HospitalVisit>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
    }

    public class Appointment
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; } = default!;
        public Guid DoctorId { get; set; }
        public User Doctor { get; set; } = default!;
        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; } = "Scheduled";
        public string Symptoms { get; set; } = "";

        public ICollection<AssignedTest> AssignedTests { get; set; } = new List<AssignedTest>();
    }   

    public class MedicalRecord
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; } = default!;
        public Guid DoctorId { get; set; }
        public User Doctor { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public string Anamnesis { get; set; } = default!;
        public string Diagnosis { get; set; } = default!;
        public string Prescriptions { get; set; } = default!;
    }

    public class HospitalVisit
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; } = default!;
        public DateTime AdmissionDate { get; set; }
        public DateTime? DischargeDate { get; set; }
        public string BedNumber { get; set; } = default!;
        public string Notes { get; set; } = default!;
    }

    public class Service
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }
        public string Category { get; set; } = default!;
    }

    public class Payment
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; } = default!;
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = default!;
        public string? ReceiptNumber { get; set; }

        public ICollection<Refund> Refunds { get; set; } = new List<Refund>();
    }

    public class Refund
    {
        public Guid Id { get; set; }
        public Guid PaymentId { get; set; }
        public Payment Payment { get; set; } = default!;
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; } = default!;
    }

    public class AssignedTest
    {
        public Guid Id { get; set; }
        public Guid AppointmentId { get; set; }
        public Appointment Appointment { get; set; } = default!;
        public Guid TestTemplateId { get; set; }
        public TestTemplate TestTemplate { get; set; } = default!;
        public ICollection<TestResult> Results { get; set; } = new List<TestResult>();
    }

    public class TestTemplate
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
    }

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

    public class QueueItem
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public string Department { get; set; } = default!;
        public string Status { get; set; } = "Waiting";
    }

}
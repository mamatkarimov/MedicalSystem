using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace MedicalSystem.Domain.Entities1
{


    public class Appointment
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; } = default!;
        public Guid DoctorId { get; set; }
        public User Doctor { get; set; } = default!;
        public DateTime AppointmentDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; } = "Scheduled";
        public string Symptoms { get; set; } = "";

        public ICollection<AssignedTest> AssignedTests { get; set; } = new List<AssignedTest>();
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






    public class Bed
    {

        [Key]
        public int BedID { get; set; }

        [Required]
        public int WardID { get; set; }

        [Required]
        public string BedNumber { get; set; }

        [Required]
        public bool IsOccupied { get; set; } = false;

        // Navigation properties
        public Ward Ward { get; set; }
        public ICollection<Hospitalization> Hospitalizations { get; set; }
    }









    public class Department
    {

        [Key]
        public int DepartmentID { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        public string HeadDoctorID { get; set; }

        // Navigation properties
        public User HeadDoctor { get; set; }
        public ICollection<Ward> Wards { get; set; }
    }








    public class Hospitalization
    {
        [Key]
        public int HospitalizationID { get; set; }

        [Required]
        public int PatientID { get; set; }

        [Required]
        public int BedID { get; set; }

        [Required]
        public DateTime AdmissionDate { get; set; }

        public DateTime? DischargeDate { get; set; }
        public string DiagnosisOnAdmission { get; set; }
        public string DiagnosisOnDischarge { get; set; }
        public string AttendingDoctorID { get; set; }

        [Required]
        public string Status { get; set; } // Active, Discharged, Transferred

        // Navigation properties
        public Patient Patient { get; set; }
        public Bed Bed { get; set; }
        public User AttendingDoctor { get; set; }
        public ICollection<NurseRound> NurseRounds { get; set; }
        public ICollection<PatientDiet> PatientDiets { get; set; }
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







    public class Invoice
    {
        [Key]
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
        public User CreatedBy { get; set; }
        public ICollection<InvoiceDetail> InvoiceDetails { get; set; }
        public ICollection<Payment> Payments { get; set; }
    }






    public class InvoiceDetail
    {
        [Key]
        public int InvoiceDetailID { get; set; }

        [Required]
        public int InvoiceID { get; set; }

        [Required]
        public Guid ServiceID { get; set; }

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








    public class LabOrder
    {
        [Key]
        public int OrderID { get; set; }

        [Required]
        public int PatientID { get; set; }

        [Required]
        public string OrderedByID { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required]
        public string Status { get; set; } = "Pending"; // Pending, InProgress, Completed, Cancelled

        [Required]
        public string Priority { get; set; } = "Routine"; // Routine, Urgent, STAT

        public string Notes { get; set; }

        // Navigation properties
        public Patient Patient { get; set; }
        public User OrderedBy { get; set; }
        public ICollection<LabOrderDetail> LabOrderDetails { get; set; }
    }








    public class LabOrderDetail
    {

        [Key]
        public int OrderDetailID { get; set; }

        [Required]
        public int OrderID { get; set; }

        [Required]
        public int TestTypeID { get; set; }

        [Required]
        public string Status { get; set; } = "Pending";

        public string Result { get; set; }
        public DateTime? ResultDate { get; set; }
        public string PerformedByID { get; set; }
        public string ReferenceRange { get; set; }

        // Navigation properties
        public LabOrder LabOrder { get; set; }
        public LabTestType TestType { get; set; }
        public User PerformedBy { get; set; }
    }





    public class LabTestType
    {
        [Key]
        public int TestTypeID { get; set; }

        [Required]
        public string TestName { get; set; }

        public string Description { get; set; }
        public string SampleType { get; set; }
        public string PreparationInstructions { get; set; }
        public string NormalRange { get; set; }

        // Navigation properties
        public ICollection<LabOrderDetail> LabOrderDetails { get; set; }
    }





    public class MedicalHistory
    {
        [Key]
        public int HistoryID { get; set; }

        [Required]
        public int PatientID { get; set; }
        public int? AppointmentID { get; set; }

        public DateTime RecordDate { get; set; } = DateTime.UtcNow;

        [Required]
        public string RecordedByID { get; set; }

        [Required]
        public string HistoryType { get; set; } // Anamnesis, Allergy, Chronic Disease, etc.

        public string Description { get; set; }

        // Navigation properties
        public Patient Patient { get; set; }
        public Appointment Appointment { get; set; }  // Добавляем навигационное свойство
        public User RecordedBy { get; set; }
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




    public class NurseRound
    {

        [Key]
        public int RoundID { get; set; }

        [Required]
        public string NurseID { get; set; }

        [Required]
        public int PatientID { get; set; }

        public DateTime RoundDate { get; set; } = DateTime.UtcNow;
        public decimal? Temperature { get; set; }
        public string BloodPressure { get; set; }
        public int? Pulse { get; set; }
        public int? RespirationRate { get; set; }
        public string Notes { get; set; }

        // Navigation properties
        public User Nurse { get; set; }
        public Patient Patient { get; set; }
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

        public bool IsActive { get; set; } = true;

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<HospitalVisit> HospitalVisits { get; set; } = new List<HospitalVisit>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
    }





    public class PatientDiet
    {

        [Key] public int DietID { get; set; }

        [Required]
        public int PatientID { get; set; }

        [Required]
        public int HospitalizationID { get; set; }

        [Required]
        public string DietType { get; set; }

        [Required]
        public DateTime StartDate { get; set; } = DateTime.UtcNow;

        public DateTime? EndDate { get; set; }
        public string Notes { get; set; }

        // Navigation properties
        public Patient Patient { get; set; }
        public Hospitalization Hospitalization { get; set; }
    }






    public class PatientQueue
    {
        [Key]
        public int QueueID { get; set; }
        [Required]
        public int PatientID { get; set; }

        public int? AppointmentID { get; set; }

        public DateTime QueueDate { get; set; } = DateTime.UtcNow;

        [Required]
        public string Status { get; set; } = "Waiting"; // Waiting, InProgress, Completed, Cancelled

        [Required]
        public int Priority { get; set; } = 5; // 1 highest, 10 lowest

        public int? DepartmentID { get; set; }
        public string Notes { get; set; }

        // Navigation properties
        public Patient Patient { get; set; }
        public Appointment Appointment { get; set; }
        public Department Department { get; set; }
    }




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




    public class Prescription
    {

        [Key]
        public int PrescriptionID { get; set; }

        [Required]
        public int PatientID { get; set; }

        [Required]
        public string PrescribedByID { get; set; }

        public DateTime PrescriptionDate { get; set; } = DateTime.UtcNow;

        [Required]
        public string Medication { get; set; }

        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public string Duration { get; set; }
        public string Instructions { get; set; }

        [Required]
        public string Status { get; set; } = "Active";

        // Navigation properties
        public Patient Patient { get; set; }
        public User PrescribedBy { get; set; }
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


    public class Refund
    {
        public Guid Id { get; set; }
        public Guid PaymentId { get; set; }
        public Payment Payment { get; set; } = default!;
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; } = default!;
    }





    public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }


    public class Service
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }
        public string Category { get; set; } = default!;
        public bool IsActive { get; set; } = true;
    }


    public class StaffProfile
    {
        public int Id { get; set; }
        public Guid? UserId { get; set; }
        public User? User { get; set; }
        public string Position { get; set; } // Example: "Doctor", "Nurse", "Admin", etc.
        public string? Department { get; set; } // Optional: e.g., "Cardiology"
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
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


    public class TestTemplate
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        //public ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
    }






    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Username { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public string Email { get; set; } = default!;
        public bool IsActive { get; set; } = true;
        //public string Role { get; set; } = default!;
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        public StaffProfile? StaffProfile { get; set; } // Optional: if the user is a staff member
    }




    public class UserRole
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = default!;
        public Guid RoleId { get; set; }
        public Role Role { get; set; } = default!;
    }






    public class Ward
    {
        [Key]
        public int WardID { get; set; }

        [Required]
        public int DepartmentID { get; set; }

        [Required]
        public string WardNumber { get; set; }

        [Required]
        public int Capacity { get; set; }

        [Required]
        public char GenderSpecific { get; set; } // 'M', 'F', or 'N'

        // Navigation properties
        public Department Department { get; set; }
        public ICollection<Bed> Beds { get; set; }
    }




}
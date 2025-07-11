using System;
using System.Collections.Generic;
using MedicalSystem.Domain.Entities1;
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
        public string Email { get; set; } = default!;
        public bool IsActive { get; set; } = true;

        public Patient Patient { get; set; } = default!;

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        //public string Role { get; set; } = default!;
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public ICollection<LabOrderDetail> LabOrderDetails { get; set; } = new List<LabOrderDetail>();
        public ICollection<LabOrder> LabOrders { get; set; } = new List<LabOrder>();
        public ICollection<Department> Departments { get; set; } = new List<Department>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
        public ICollection<MedicalHistory> MedicalHistories { get; set; } = new List<MedicalHistory>();
        public ICollection<NurseRound> NurseRounds { get; set; } = new List<NurseRound>();
        public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
        public ICollection<Hospitalization> Hospitalizations { get; set; } = new List<Hospitalization>();
        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
        
        public StaffProfile? StaffProfile { get; set; } // Optional: if the user is a staff member
    }

}
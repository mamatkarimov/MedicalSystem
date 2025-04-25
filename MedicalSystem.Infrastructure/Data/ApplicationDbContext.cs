using MedicalSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace MedicalSystem.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Patients module
        public DbSet<Patient> Patients { get; set; }
        public DbSet<PatientDocument> PatientDocuments { get; set; }

        // Appointments module
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<MedicalHistory> MedicalHistories { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }

        // Laboratory module
        public DbSet<LabTestType> LabTestTypes { get; set; }
        public DbSet<LabOrder> LabOrders { get; set; }
        public DbSet<LabOrderDetail> LabOrderDetails { get; set; }
        public DbSet<InstrumentalStudy> InstrumentalStudies { get; set; }

        // Stationary module
        public DbSet<Department> Departments { get; set; }
        public DbSet<Ward> Wards { get; set; }
        public DbSet<Bed> Beds { get; set; }
        public DbSet<Hospitalization> Hospitalizations { get; set; }
        public DbSet<NurseRound> NurseRounds { get; set; }
        public DbSet<PatientDiet> PatientDiets { get; set; }

        // Payments module
        public DbSet<Service> Services { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        public DbSet<Payment> Payments { get; set; }

        // Queue module
        public DbSet<PatientQueue> PatientQueues { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            // Configure relationships and constraints

            // Patient relationships
            builder.Entity<Patient>()
                .HasMany(p => p.Appointments)
                .WithOne(a => a.Patient)
                .HasForeignKey(a => a.PatientID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Patient>()
                .HasMany(p => p.Hospitalizations)
                .WithOne(h => h.Patient)
                .HasForeignKey(h => h.PatientID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Patient>()
                .HasMany(p => p.PatientDocuments)
                .WithOne(pd => pd.Patient)
                .HasForeignKey(pd => pd.PatientID)
                .OnDelete(DeleteBehavior.Cascade);

            // Appointment relationships
            builder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany()
                .HasForeignKey(a => a.DoctorID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Appointment>()
                .HasMany(a => a.MedicalHistories)
                .WithOne(mh => mh.Appointment)
                .HasForeignKey(mh => mh.AppointmentID)
                .OnDelete(DeleteBehavior.Cascade);

            // Medical History relationships
            builder.Entity<MedicalHistory>()
                .HasOne(mh => mh.RecordedBy)
                .WithMany()
                .HasForeignKey(mh => mh.RecordedByID)
                .OnDelete(DeleteBehavior.Restrict);

            // Prescription relationships
            builder.Entity<Prescription>()
                .HasOne(p => p.PrescribedBy)
                .WithMany()
                .HasForeignKey(p => p.PrescribedByID)
                .OnDelete(DeleteBehavior.Restrict);

            // Laboratory relationships
            builder.Entity<LabOrder>()
                .HasOne(lo => lo.OrderedBy)
                .WithMany()
                .HasForeignKey(lo => lo.OrderedByID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<LabOrder>()
                .HasMany(lo => lo.LabOrderDetails)
                .WithOne(lod => lod.LabOrder)
                .HasForeignKey(lod => lod.OrderID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<LabOrderDetail>()
                .HasOne(lod => lod.PerformedBy)
                .WithMany()
                .HasForeignKey(lod => lod.PerformedByID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<LabOrderDetail>()
                .HasOne(lod => lod.TestType)
                .WithMany(tt => tt.LabOrderDetails)
                .HasForeignKey(lod => lod.TestTypeID)
                .OnDelete(DeleteBehavior.Restrict);

            // Instrumental Studies relationships
            builder.Entity<InstrumentalStudy>()
                .HasOne(isu => isu.OrderedBy)
                .WithMany()
                .HasForeignKey(isu => isu.OrderedByID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<InstrumentalStudy>()
                .HasOne(isu => isu.PerformedBy)
                .WithMany()
                .HasForeignKey(isu => isu.PerformedByID)
                .OnDelete(DeleteBehavior.Restrict);

            // Stationary relationships
            builder.Entity<Department>()
                .HasOne(d => d.HeadDoctor)
                .WithMany()
                .HasForeignKey(d => d.HeadDoctorID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Ward>()
                .HasOne(w => w.Department)
                .WithMany(d => d.Wards)
                .HasForeignKey(w => w.DepartmentID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Bed>()
                .HasOne(b => b.Ward)
                .WithMany(w => w.Beds)
                .HasForeignKey(b => b.WardID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Hospitalization>()
                .HasOne(h => h.Bed)
                .WithMany(b => b.Hospitalizations)
                .HasForeignKey(h => h.BedID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Hospitalization>()
                .HasOne(h => h.AttendingDoctor)
                .WithMany()
                .HasForeignKey(h => h.AttendingDoctorID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<NurseRound>()
                .HasOne(nr => nr.Nurse)
                .WithMany()
                .HasForeignKey(nr => nr.NurseID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PatientDiet>()
                .HasOne(pd => pd.Hospitalization)
                .WithMany(h => h.PatientDiets)
                .HasForeignKey(pd => pd.HospitalizationID)
                .OnDelete(DeleteBehavior.Restrict);

            // Payments relationships
            builder.Entity<Invoice>()
                .HasOne(i => i.CreatedBy)
                .WithMany()
                .HasForeignKey(i => i.CreatedByID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Invoice>()
                .HasMany(i => i.InvoiceDetails)
                .WithOne(id => id.Invoice)
                .HasForeignKey(id => id.InvoiceID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Invoice>()
                .HasMany(i => i.Payments)
                .WithOne(p => p.Invoice)
                .HasForeignKey(p => p.InvoiceID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<InvoiceDetail>()
                .HasOne(id => id.Service)
                .WithMany()
                .HasForeignKey(id => id.ServiceID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Payment>()
                .HasOne(p => p.ReceivedBy)
                .WithMany()
                .HasForeignKey(p => p.ReceivedByID)
                .OnDelete(DeleteBehavior.Restrict);

            // Queue relationships
            builder.Entity<PatientQueue>()
                .HasOne(pq => pq.Department)
                .WithMany()
                .HasForeignKey(pq => pq.DepartmentID)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure enums and value conversions
            builder.Entity<Patient>()
                .Property(p => p.Gender)
                .HasConversion<string>()
                .HasMaxLength(1);

            builder.Entity<Ward>()
                .Property(w => w.GenderSpecific)
                .HasConversion<string>()
                .HasMaxLength(1);

            // Configure indexes for performance
            builder.Entity<Patient>()
                .HasIndex(p => new { p.LastName, p.FirstName, p.MiddleName });

            builder.Entity<Patient>()
                .HasIndex(p => p.BirthDate);

            builder.Entity<Patient>()
                .HasIndex(p => p.InsuranceNumber);

            builder.Entity<Appointment>()
                .HasIndex(a => a.PatientID);

            builder.Entity<Appointment>()
                .HasIndex(a => a.DoctorID);

            builder.Entity<Appointment>()
                .HasIndex(a => new { a.AppointmentDate, a.Status });

            builder.Entity<Hospitalization>()
                .HasIndex(h => h.PatientID);

            builder.Entity<Hospitalization>()
                .HasIndex(h => h.BedID);

            builder.Entity<Hospitalization>()
                .HasIndex(h => h.Status);

            builder.Entity<LabOrder>()
                .HasIndex(lo => lo.PatientID);

            builder.Entity<LabOrder>()
                .HasIndex(lo => lo.Status);

            builder.Entity<PatientQueue>()
                .HasIndex(pq => pq.Status);

            builder.Entity<PatientQueue>()
                .HasIndex(pq => pq.DepartmentID);
        }
    }
}
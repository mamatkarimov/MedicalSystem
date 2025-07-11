using MedicalSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedicalSystem.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }
         
        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<StaffProfile> StaffProfiles { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<HospitalVisit> HospitalVisits { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Refund> Refunds { get; set; }
        public DbSet<AssignedTest> AssignedTests { get; set; }
        public DbSet<TestTemplate> TestTemplates { get; set; }
        public DbSet<TestResult> TestResults { get; set; }
        public DbSet<QueueItem> QueueItems { get; set; }
        public DbSet<Hospitalization> Hospitalizations { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<NurseRound> NurseRounds { get; set; }
        public DbSet<PatientDiet> PatientDiets { get; set; }
        public DbSet<Ward> Wards { get; set; }
        public DbSet<PatientQueue> PatientQueues { get; set; }
        public DbSet<LabOrder> LabOrders { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<MedicalHistory> MedicalHistories { get; set; }
        public DbSet<LabTestType> LabTestTypes { get; set; }
        public DbSet<LabOrderDetail> LabOrderDetails { get; set; }
        public DbSet<Bed> Beds { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureEntitiesByAI(modelBuilder);
        }

        private static void ConfigureEntitiesByAI(ModelBuilder modelBuilder)
        {
            // User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Username).IsRequired().HasMaxLength(50);
                entity.Property(u => u.PasswordHash).IsRequired();
                entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
                entity.Property(u => u.IsActive).HasDefaultValue(true);

                entity.HasIndex(u => u.Username).IsUnique();
                entity.HasIndex(u => u.Email).IsUnique();

                entity.HasMany(u => u.UserRoles)
                      .WithOne(ur => ur.User)
                      .HasForeignKey(ur => ur.UserId);

                entity.HasOne(u => u.StaffProfile)
                      .WithOne(sp => sp.User)
                      .HasForeignKey<StaffProfile>(sp => sp.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Name).IsRequired().HasMaxLength(50);

                entity.HasMany(r => r.UserRoles)
                      .WithOne(ur => ur.Role)
                      .HasForeignKey(ur => ur.RoleId);
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(ur => new { ur.UserId, ur.RoleId });
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(p => p.LastName).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Gender).IsRequired();
                entity.Property(p => p.IsActive).HasDefaultValue(true);

                entity.HasOne(p => p.User)
                      .WithOne(u => u.Patient)
                      .HasForeignKey<Patient>(p => p.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<StaffProfile>(entity =>
            {
                entity.HasKey(sp => sp.Id);
                entity.Property(sp => sp.Position).IsRequired();
                entity.Property(sp => sp.FirstName).IsRequired();
                entity.Property(sp => sp.LastName).IsRequired();
            });

            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Status).IsRequired().HasMaxLength(50);
                entity.Property(a => a.Symptoms).HasMaxLength(500);

                entity.HasOne(a => a.Patient)
                      .WithMany(p => p.Appointments)
                      .HasForeignKey(a => a.PatientId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.Doctor)
                      .WithMany(d => d.Appointments)
                      .HasForeignKey(a => a.DoctorId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<AssignedTest>(entity =>
            {
                entity.HasKey(at => at.Id);
                entity.HasOne(at => at.Appointment)
                      .WithMany(a => a.AssignedTests)
                      .HasForeignKey(at => at.AppointmentId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(at => at.TestTemplate)
                      .WithMany(tt => tt.AssignedTests)
                      .HasForeignKey(at => at.TestTemplateId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TestTemplate>(entity =>
            {
                entity.HasKey(tt => tt.Id);
                entity.Property(tt => tt.Name).IsRequired();
            });

            modelBuilder.Entity<TestResult>(entity =>
            {
                entity.HasKey(tr => tr.Id);
                entity.Property(tr => tr.ParameterName).IsRequired();
                entity.Property(tr => tr.Value).IsRequired();
                entity.Property(tr => tr.Unit).IsRequired();
                entity.Property(tr => tr.ReferenceRange).IsRequired();
                entity.HasOne(tr => tr.AssignedTest)
                      .WithMany(at => at.Results)
                      .HasForeignKey(tr => tr.AssignedTestId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Bed>(entity =>
            {
                entity.HasKey(b => b.BedID);
                entity.Property(b => b.BedNumber).IsRequired();
                entity.HasOne(b => b.Ward)
                      .WithMany(w => w.Beds)
                      .HasForeignKey(b => b.WardID)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(d => d.DepartmentID);
                entity.Property(d => d.Name).IsRequired();
                entity.HasOne(d => d.HeadDoctor)
                      .WithMany(hd => hd.Departments)
                      .HasForeignKey(d => d.HeadDoctorID)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Ward>(entity =>
            {
                entity.HasKey(w => w.WardID);
                entity.Property(w => w.WardNumber).IsRequired();
                entity.HasOne(w => w.Department)
                      .WithMany(d => d.Wards)
                      .HasForeignKey(w => w.DepartmentID)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Hospitalization>(entity =>
            {
                entity.HasKey(h => h.HospitalizationID);
                entity.HasOne(h => h.Patient)
                      .WithMany(p => p.Hospitalizations)
                      .HasForeignKey(h => h.PatientID)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(h => h.Bed)
                      .WithMany(b => b.Hospitalizations)
                      .HasForeignKey(h => h.BedID)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(h => h.AttendingDoctor)
                      .WithMany(d => d.Hospitalizations)
                      .HasForeignKey(h => h.AttendingDoctorID)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<HospitalVisit>(entity =>
            {
                entity.HasKey(hv => hv.Id);
                entity.HasOne(hv => hv.Patient)
                      .WithMany(p => p.HospitalVisits)
                      .HasForeignKey(hv => hv.PatientId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.HasOne(p => p.Patient)
                      .WithMany(pat => pat.Payments)
                      .HasForeignKey(p => p.PatientId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(p => p.Invoice)
                      .WithMany(i => i.Payments)
                      .HasForeignKey(p => p.InvoiceId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(p => p.ReceivedBy)
                      .WithMany(u => u.Payments)
                      .HasForeignKey(p => p.ReceivedByID)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Refund>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.HasOne(r => r.Payment)
                      .WithMany(p => p.Refunds)
                      .HasForeignKey(r => r.PaymentId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.HasKey(i => i.InvoiceID);
                entity.HasOne(i => i.Patient)
                      .WithMany(p => p.Invoices)
                      .HasForeignKey(i => i.PatientID)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(i => i.CreatedBy)
                      .WithMany(u => u.Invoices)
                      .HasForeignKey(i => i.CreatedByID)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<InvoiceDetail>(entity =>
            {
                entity.HasKey(id => id.InvoiceDetailID);
                entity.HasOne(id => id.Invoice)
                      .WithMany(i => i.InvoiceDetails)
                      .HasForeignKey(id => id.InvoiceID)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(id => id.Service)
                      .WithMany(s => s.InvoiceDetails)
                      .HasForeignKey(id => id.ServiceID)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.HasKey(s => s.Id);
            });

            modelBuilder.Entity<QueueItem>(entity =>
            {
                entity.HasKey(q => q.Id);
                entity.HasOne(q => q.Patient)
                      .WithMany(p => p.QueueItems)
                      .HasForeignKey(q => q.PatientId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<PatientQueue>(entity =>
            {
                entity.HasKey(pq => pq.QueueID);
                entity.HasOne(pq => pq.Patient)
                      .WithMany(p => p.PatientQueues)
                      .HasForeignKey(pq => pq.PatientID)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(pq => pq.Appointment)
                      .WithMany(a => a.PatientQueues)
                      .HasForeignKey(pq => pq.AppointmentID)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(pq => pq.Department)
                      .WithMany(d => d.PatientQueues)
                      .HasForeignKey(pq => pq.DepartmentID)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<MedicalRecord>(entity =>
            {
                entity.HasKey(mr => mr.Id);
                entity.HasOne(mr => mr.Patient)
                      .WithMany(p => p.MedicalRecords)
                      .HasForeignKey(mr => mr.PatientId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(mr => mr.Doctor)
                      .WithMany(d => d.MedicalRecords)
                      .HasForeignKey(mr => mr.DoctorId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<MedicalHistory>(entity =>
            {
                entity.HasKey(mh => mh.HistoryID);
                entity.HasOne(mh => mh.Patient)
                      .WithMany(p => p.MedicalHistories)
                      .HasForeignKey(mh => mh.PatientID)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(mh => mh.Appointment)
                      .WithMany(a => a.MedicalHistories)
                      .HasForeignKey(mh => mh.AppointmentID)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(mh => mh.RecordedBy)
                      .WithMany(u => u.MedicalHistories)
                      .HasForeignKey(mh => mh.RecordedByID)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<NurseRound>(entity =>
            {
                entity.HasKey(nr => nr.RoundID);
                entity.HasOne(nr => nr.Nurse)
                      .WithMany(n => n.NurseRounds)
                      .HasForeignKey(nr => nr.NurseID)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(nr => nr.Patient)
                      .WithMany(p => p.NurseRounds)
                      .HasForeignKey(nr => nr.PatientID)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<PatientDiet>(entity =>
            {
                entity.HasKey(pd => pd.DietID);
                entity.HasOne(pd => pd.Patient)
                      .WithMany(p => p.PatientDiets)
                      .HasForeignKey(pd => pd.PatientID)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(pd => pd.Hospitalization)
                      .WithMany(h => h.PatientDiets)
                      .HasForeignKey(pd => pd.HospitalizationID)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<LabOrder>(entity =>
            {
                entity.HasKey(lo => lo.Id);
                entity.HasOne(lo => lo.Patient)
                      .WithMany(p => p.LabOrders)
                      .HasForeignKey(lo => lo.PatientID)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(lo => lo.OrderedBy)
                      .WithMany(u => u.LabOrders)
                      .HasForeignKey(lo => lo.OrderedByID)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<LabOrderDetail>(entity =>
            {
                entity.HasKey(lod => lod.Id);
                entity.HasOne(lod => lod.LabOrder)
                      .WithMany(lo => lo.LabOrderDetails)
                      .HasForeignKey(lod => lod.OrderId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(lod => lod.TestType)
                      .WithMany(tt => tt.LabOrderDetails)
                      .HasForeignKey(lod => lod.TestTypeId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(lod => lod.PerformedBy)
                      .WithMany(u => u.LabOrderDetails)
                      .HasForeignKey(lod => lod.PerformedById)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Prescription>(entity =>
            {
                entity.HasKey(p => p.PrescriptionID);
                entity.HasOne(p => p.Patient)
                      .WithMany(p => p.Prescriptions)
                      .HasForeignKey(p => p.PatientID)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(p => p.PrescribedBy)
                      .WithMany(u => u.Prescriptions)
                      .HasForeignKey(p => p.PrescribedByID)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }

    }
}
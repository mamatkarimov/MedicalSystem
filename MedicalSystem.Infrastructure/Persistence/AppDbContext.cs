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
                      .HasForeignKey<StaffProfile>(sp => sp.UserId);
            });

            // Role
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Name).IsRequired().HasMaxLength(50);

                entity.HasMany(r => r.UserRoles)
                      .WithOne(ur => ur.Role)
                      .HasForeignKey(ur => ur.RoleId);
            });

            // UserRole
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(ur => new { ur.UserId, ur.RoleId });
            });

            // Patient
            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(p => p.LastName).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Gender).IsRequired();
                entity.Property(p => p.IsActive).HasDefaultValue(true);

                entity.HasOne(p => p.User)
                      .WithOne()
                      .HasForeignKey<Patient>(p => p.UserId);
            });

            // StaffProfile
            modelBuilder.Entity<StaffProfile>(entity =>
            {
                entity.HasKey(sp => sp.Id);
                entity.Property(sp => sp.Position).IsRequired();
                entity.Property(sp => sp.FirstName).IsRequired();
                entity.Property(sp => sp.LastName).IsRequired();
            });

            // Appointment
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
                      .WithMany()
                      .HasForeignKey(a => a.DoctorId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // AssignedTest
            modelBuilder.Entity<AssignedTest>(entity =>
            {
                entity.HasKey(at => at.Id);
                entity.HasOne(at => at.Appointment)
                      .WithMany(a => a.AssignedTests)
                      .HasForeignKey(at => at.AppointmentId);
                entity.HasOne(at => at.TestTemplate)
                      .WithMany()
                      .HasForeignKey(at => at.TestTemplateId);
            });

            // TestTemplate
            modelBuilder.Entity<TestTemplate>(entity =>
            {
                entity.HasKey(tt => tt.Id);
                entity.Property(tt => tt.Name).IsRequired();
            });

            // TestResult
            modelBuilder.Entity<TestResult>(entity =>
            {
                entity.HasKey(tr => tr.Id);
                entity.Property(tr => tr.ParameterName).IsRequired();
                entity.Property(tr => tr.Value).IsRequired();
                entity.Property(tr => tr.Unit).IsRequired();
                entity.Property(tr => tr.ReferenceRange).IsRequired();
                entity.HasOne(tr => tr.AssignedTest)
                      .WithMany(at => at.Results)
                      .HasForeignKey(tr => tr.AssignedTestId);
            });

            // Bed
            modelBuilder.Entity<Bed>(entity =>
            {
                entity.HasKey(b => b.BedID);
                entity.Property(b => b.BedNumber).IsRequired();
                entity.HasOne(b => b.Ward)
                      .WithMany(w => w.Beds)
                      .HasForeignKey(b => b.WardID);
            });

            // Department
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(d => d.DepartmentID);
                entity.Property(d => d.Name).IsRequired();
                entity.HasOne(d => d.HeadDoctor)
                      .WithMany()
                      .HasForeignKey(d => d.HeadDoctorID);
            });

            // Ward
            modelBuilder.Entity<Ward>(entity =>
            {
                entity.HasKey(w => w.WardID);
                entity.Property(w => w.WardNumber).IsRequired();
                entity.HasOne(w => w.Department)
                      .WithMany(d => d.Wards)
                      .HasForeignKey(w => w.DepartmentID);
            });

            // Hospitalization
            modelBuilder.Entity<Hospitalization>(entity =>
            {
                entity.HasKey(h => h.HospitalizationID);
                entity.HasOne(h => h.Patient)
                      .WithMany()
                      .HasForeignKey(h => h.PatientID);
                entity.HasOne(h => h.Bed)
                      .WithMany(b => b.Hospitalizations)
                      .HasForeignKey(h => h.BedID);
                entity.HasOne(h => h.AttendingDoctor)
                      .WithMany()
                      .HasForeignKey(h => h.AttendingDoctorID);
            });

            // HospitalVisit
            modelBuilder.Entity<HospitalVisit>(entity =>
            {
                entity.HasKey(hv => hv.Id);
                entity.HasOne(hv => hv.Patient)
                      .WithMany(p => p.HospitalVisits)
                      .HasForeignKey(hv => hv.PatientId);
            });

            // Payment
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.HasOne(p => p.Patient)
                      .WithMany(pat => pat.Payments)
                      .HasForeignKey(p => p.PatientId);
                entity.HasOne(p => p.Invoice)
                      .WithMany(i => i.Payments)
                      .HasForeignKey(p => p.InvoiceId);
                entity.HasOne(p => p.ReceivedBy)
                      .WithMany()
                      .HasForeignKey(p => p.ReceivedByID);
            });

            // Refund
            modelBuilder.Entity<Refund>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.HasOne(r => r.Payment)
                      .WithMany(p => p.Refunds)
                      .HasForeignKey(r => r.PaymentId);
            });

            // Invoice
            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.HasKey(i => i.InvoiceID);
                entity.HasOne(i => i.Patient)
                      .WithMany()
                      .HasForeignKey(i => i.PatientID);
                entity.HasOne(i => i.CreatedBy)
                      .WithMany()
                      .HasForeignKey(i => i.CreatedByID);
            });

            // InvoiceDetail
            modelBuilder.Entity<InvoiceDetail>(entity =>
            {
                entity.HasKey(id => id.InvoiceDetailID);
                entity.HasOne(id => id.Invoice)
                      .WithMany(i => i.InvoiceDetails)
                      .HasForeignKey(id => id.InvoiceID);
                entity.HasOne(id => id.Service)
                      .WithMany()
                      .HasForeignKey(id => id.ServiceID);
            });

            // Service
            modelBuilder.Entity<Service>(entity =>
            {
                entity.HasKey(s => s.Id);
            });

            // QueueItem
            modelBuilder.Entity<QueueItem>(entity =>
            {
                entity.HasKey(q => q.Id);
                entity.HasOne(q => q.Patient)
                      .WithMany()
                      .HasForeignKey(q => q.PatientId);
            });

            // PatientQueue
            modelBuilder.Entity<PatientQueue>(entity =>
            {
                entity.HasKey(pq => pq.QueueID);
                entity.HasOne(pq => pq.Patient)
                      .WithMany()
                      .HasForeignKey(pq => pq.PatientID);
                entity.HasOne(pq => pq.Appointment)
                      .WithMany()
                      .HasForeignKey(pq => pq.AppointmentID);
                entity.HasOne(pq => pq.Department)
                      .WithMany()
                      .HasForeignKey(pq => pq.DepartmentID);
            });

            // MedicalRecord
            modelBuilder.Entity<MedicalRecord>(entity =>
            {
                entity.HasKey(mr => mr.Id);
                entity.HasOne(mr => mr.Patient)
                      .WithMany(p => p.MedicalRecords)
                      .HasForeignKey(mr => mr.PatientId);
                entity.HasOne(mr => mr.Doctor)
                      .WithMany()
                      .HasForeignKey(mr => mr.DoctorId);
            });

            // MedicalHistory
            modelBuilder.Entity<MedicalHistory>(entity =>
            {
                entity.HasKey(mh => mh.HistoryID);
                entity.HasOne(mh => mh.Patient)
                      .WithMany()
                      .HasForeignKey(mh => mh.PatientID);
                entity.HasOne(mh => mh.Appointment)
                      .WithMany()
                      .HasForeignKey(mh => mh.AppointmentID);
                entity.HasOne(mh => mh.RecordedBy)
                      .WithMany()
                      .HasForeignKey(mh => mh.RecordedByID);
            });

            // NurseRound
            modelBuilder.Entity<NurseRound>(entity =>
            {
                entity.HasKey(nr => nr.RoundID);
                entity.HasOne(nr => nr.Nurse)
                      .WithMany()
                      .HasForeignKey(nr => nr.NurseID);
                entity.HasOne(nr => nr.Patient)
                      .WithMany()
                      .HasForeignKey(nr => nr.PatientID);
            });

            // PatientDiet
            modelBuilder.Entity<PatientDiet>(entity =>
            {
                entity.HasKey(pd => pd.DietID);
                entity.HasOne(pd => pd.Patient)
                      .WithMany()
                      .HasForeignKey(pd => pd.PatientID);
                entity.HasOne(pd => pd.Hospitalization)
                      .WithMany(h => h.PatientDiets)
                      .HasForeignKey(pd => pd.HospitalizationID);
            });

            // LabOrder
            modelBuilder.Entity<LabOrder>(entity =>
            {
                entity.HasKey(lo => lo.OrderID);
                entity.HasOne(lo => lo.Patient)
                      .WithMany()
                      .HasForeignKey(lo => lo.PatientID);
                entity.HasOne(lo => lo.OrderedBy)
                      .WithMany()
                      .HasForeignKey(lo => lo.OrderedByID);
            });

            // LabOrderDetail
            modelBuilder.Entity<LabOrderDetail>(entity =>
            {
                entity.HasKey(lod => lod.OrderDetailID);
                entity.HasOne(lod => lod.LabOrder)
                      .WithMany(lo => lo.LabOrderDetails)
                      .HasForeignKey(lod => lod.OrderID)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(lod => lod.TestType)
                      .WithMany(tt => tt.LabOrderDetails)
                      .HasForeignKey(lod => lod.TestTypeID)
                      .OnDelete(DeleteBehavior.NoAction) ;
                entity.HasOne(lod => lod.PerformedBy)
                      .WithMany()
                      .HasForeignKey(lod => lod.PerformedByID)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            // Prescription
            modelBuilder.Entity<Prescription>(entity =>
            {
                entity.HasKey(p => p.PrescriptionID);
                entity.HasOne(p => p.Patient)
                      .WithMany()
                      .HasForeignKey(p => p.PatientID);
                entity.HasOne(p => p.PrescribedBy)
                      .WithMany()
                      .HasForeignKey(p => p.PrescribedByID);
            });
        }

        private static void ConfigureEntities(ModelBuilder modelBuilder)
        {
            // User Configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Username).IsRequired().HasMaxLength(50);
                entity.Property(u => u.PasswordHash).IsRequired();
                entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
                entity.Property(u => u.IsActive).HasDefaultValue(true);

                // Indexes
                entity.HasIndex(u => u.Username).IsUnique();
                entity.HasIndex(u => u.Email).IsUnique();

                // Relationships
                entity.HasMany(u => u.UserRoles)
                      .WithOne(ur => ur.User)
                      .HasForeignKey(ur => ur.UserId);
            });

            // Role Configuration
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Name).IsRequired().HasMaxLength(50);

                // Relationships
                entity.HasMany(r => r.UserRoles)
                      .WithOne(ur => ur.Role)
                      .HasForeignKey(ur => ur.RoleId);
            });

            // UserRole (junction table) Configuration
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(ur => new { ur.UserId, ur.RoleId });

                // Relationships
                entity.HasOne(ur => ur.User)
                      .WithMany(u => u.UserRoles)
                      .HasForeignKey(ur => ur.UserId);

                entity.HasOne(ur => ur.Role)
                      .WithMany(r => r.UserRoles)
                      .HasForeignKey(ur => ur.RoleId);
            });

            // Patient Configuration
            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(p => p.LastName).IsRequired().HasMaxLength(50);
                entity.Property(p => p.Gender).IsRequired().HasMaxLength(10);

                // Relationships
                entity.HasOne(p => p.User)
                      .WithMany()
                      .HasForeignKey(p => p.UserId)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(p => p.Appointments)
                      .WithOne(a => a.Patient)
                      .HasForeignKey(a => a.PatientId);

                entity.HasMany(p => p.HospitalVisits)
                      .WithOne(hv => hv.Patient)
                      .HasForeignKey(hv => hv.PatientId);

                entity.HasMany(p => p.Payments)
                      .WithOne(p => p.Patient)
                      .HasForeignKey(p => p.PatientId);

                entity.HasMany(p => p.MedicalRecords)
                      .WithOne(mr => mr.Patient)
                      .HasForeignKey(mr => mr.PatientId);
            });

            // Patient Configuration
            modelBuilder.Entity<StaffProfile>(entity =>
            {
                entity.HasKey(p => p.Id);
                //entity.Property(p => p.FirstName).IsRequired().HasMaxLength(50);
                //entity.Property(p => p.LastName).IsRequired().HasMaxLength(50);
                //entity.Property(p => p.Gender).IsRequired().HasMaxLength(10);

                // Relationships
                entity.HasOne(p => p.User)
                      .WithMany()
                      .HasForeignKey(p => p.UserId)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Appointment Configuration
            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Status).HasDefaultValue("Scheduled");
                entity.Property(a => a.Symptoms).HasDefaultValue("");

                // Relationships
                entity.HasOne(a => a.Patient)
                      .WithMany(p => p.Appointments)
                      .HasForeignKey(a => a.PatientId);

                entity.HasOne(a => a.Doctor)
                      .WithMany()
                      .HasForeignKey(a => a.DoctorId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(a => a.AssignedTests)
                      .WithOne(at => at.Appointment)
                      .HasForeignKey(at => at.AppointmentId);
            });

            // MedicalRecord Configuration
            modelBuilder.Entity<MedicalRecord>(entity =>
            {
                entity.HasKey(mr => mr.Id);
                entity.Property(mr => mr.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(mr => mr.Anamnesis).IsRequired();
                entity.Property(mr => mr.Diagnosis).IsRequired();
                entity.Property(mr => mr.Prescriptions).IsRequired();

                // Relationships
                entity.HasOne(mr => mr.Patient)
                      .WithMany(p => p.MedicalRecords)
                      .HasForeignKey(mr => mr.PatientId);

                entity.HasOne(mr => mr.Doctor)
                      .WithMany()
                      .HasForeignKey(mr => mr.DoctorId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // HospitalVisit Configuration
            modelBuilder.Entity<HospitalVisit>(entity =>
            {
                entity.HasKey(hv => hv.Id);
                entity.Property(hv => hv.Notes).IsRequired();
                entity.Property(hv => hv.BedNumber).IsRequired();

                // Relationships
                entity.HasOne(hv => hv.Patient)
                      .WithMany(p => p.HospitalVisits)
                      .HasForeignKey(hv => hv.PatientId);
            });

            // Service Configuration
            modelBuilder.Entity<Service>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Name).IsRequired().HasMaxLength(100);
                entity.Property(s => s.Category).IsRequired().HasMaxLength(50);
                entity.Property(s => s.Price).HasColumnType("decimal(18,2)");
            });

            // Payment Configuration
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Amount).HasColumnType("decimal(18,2)");
                entity.Property(p => p.PaymentMethod).IsRequired().HasMaxLength(50);

                // Relationships
                entity.HasOne(p => p.Patient)
                      .WithMany(p => p.Payments)
                      .HasForeignKey(p => p.PatientId);

                entity.HasMany(p => p.Refunds)
                      .WithOne(r => r.Payment)
                      .HasForeignKey(r => r.PaymentId);
            });

            // Refund Configuration
            modelBuilder.Entity<Refund>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Amount).HasColumnType("decimal(18,2)");
                entity.Property(r => r.Reason).IsRequired();

                // Relationships
                entity.HasOne(r => r.Payment)
                      .WithMany(p => p.Refunds)
                      .HasForeignKey(r => r.PaymentId);
            });

            // AssignedTest Configuration
            modelBuilder.Entity<AssignedTest>(entity =>
            {
                entity.HasKey(at => at.Id);

                // Relationships
                entity.HasOne(at => at.Appointment)
                      .WithMany(a => a.AssignedTests)
                      .HasForeignKey(at => at.AppointmentId);

                entity.HasOne(at => at.TestTemplate)
                      .WithMany()
                      .HasForeignKey(at => at.TestTemplateId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(at => at.Results)
                      .WithOne(tr => tr.AssignedTest)
                      .HasForeignKey(tr => tr.AssignedTestId);
            });

            // TestTemplate Configuration
            modelBuilder.Entity<TestTemplate>(entity =>
            {
                entity.HasKey(tt => tt.Id);
                entity.Property(tt => tt.Name).IsRequired();
                entity.Property(tt => tt.Description).IsRequired();

                // Relationships
                //entity.HasMany(tt => tt.TestResults)
                //      .WithOne(tr => tr.TestTemplate)
                //      .HasForeignKey(tr => tr.TestTemplateId);
            });

            // TestResult Configuration
            modelBuilder.Entity<TestResult>(entity =>
            {
                entity.HasKey(tr => tr.Id);
                entity.Property(tr => tr.ParameterName).IsRequired();
                entity.Property(tr => tr.Value).IsRequired();
                entity.Property(tr => tr.Unit).IsRequired();
                entity.Property(tr => tr.ReferenceRange).IsRequired();

                // Relationships
                entity.HasOne(tr => tr.AssignedTest)
                      .WithMany(at => at.Results)
                      .HasForeignKey(tr => tr.AssignedTestId);

                //entity.HasOne(tr => tr.TestTemplate)
                //      .WithMany(tt => tt.TestResults)
                //      .HasForeignKey(tr => tr.TestTemplateId);
            });

            // QueueItem Configuration
            modelBuilder.Entity<QueueItem>(entity =>
            {
                entity.HasKey(qi => qi.Id);
                entity.Property(qi => qi.Department).IsRequired();
                entity.Property(qi => qi.Status).HasDefaultValue("Waiting");
                entity.Property(qi => qi.CreatedAt).HasDefaultValueSql("GETDATE()");

                // Relationships
                entity.HasOne(qi => qi.Patient)
                      .WithMany()
                      .HasForeignKey(qi => qi.PatientId);
            });
        }
    }
}
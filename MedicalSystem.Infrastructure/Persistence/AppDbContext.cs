using MedicalSystem.Domain.Entities;
using MedicalSystem.Infrastructure.Data.Configurations;
using MedicalSystem.Infrastructure.Data;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User Configuration
            modelBuilder.Entity<User>(entity =>
                {
                    entity.HasKey(u => u.Id);
                    entity.Property(u => u.Username).IsRequired().HasMaxLength(50);
                    entity.Property(u => u.PasswordHash).IsRequired();
                    entity.Property(u => u.FullName).IsRequired().HasMaxLength(100);
                    entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
                    entity.Property(u => u.IsActive).HasDefaultValue(true);
                    entity.Property(u => u.Role).HasMaxLength(50);

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
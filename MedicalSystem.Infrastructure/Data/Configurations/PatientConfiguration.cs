// MedicalSystem.Infrastructure/Data/Configurations/PatientConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MedicalSystem.Domain.Entities;

namespace MedicalSystem.Infrastructure.Data.Configurations
{
    //public class PatientConfiguration : IEntityTypeConfiguration<Patient>
    //{
    //    public void Configure(EntityTypeBuilder<Patient> builder)
    //    {
    //        builder.HasKey(p => p.PatientID);
    //        builder.Property(p => p.FirstName).IsRequired().HasMaxLength(100);
    //        builder.Property(p => p.LastName).IsRequired().HasMaxLength(100);
    //        builder.Property(p => p.BirthDate).IsRequired();
    //        builder.Property(p => p.Gender).IsRequired().HasMaxLength(1);            
    //    }
    //}
}
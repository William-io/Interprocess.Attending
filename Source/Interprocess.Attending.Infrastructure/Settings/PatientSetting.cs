using Interprocess.Attending.Domain.Patients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Interprocess.Attending.Infrastructure.Settings;

internal sealed class PatientSetting : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        // Configurações do Patient
        builder.ToTable("Patients");

        builder.HasKey(user => user.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Cpf)
            .IsRequired()
            .HasMaxLength(11);

        builder.Property(p => p.DateBirth)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(p => p.Sex)
           .IsRequired()
           .HasConversion(
               status => status.ToString(),
               status => Enum.Parse<Sex>(status));

        builder.Property(p => p.Status)
            .IsRequired()
            .HasConversion(
                status => status.ToString(),
                status => Enum.Parse<PatientStatus>(status));

        builder.OwnsOne(p => p.Address, addressBuilder =>
        {
            addressBuilder.Property(a => a.Street)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("Street");

            addressBuilder.Property(a => a.City)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("City");

            addressBuilder.Property(a => a.State)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("State");

            addressBuilder.Property(a => a.ZipCode)
                .IsRequired()
                .HasMaxLength(10)
                .HasColumnName("ZipCode");

            addressBuilder.Property(a => a.District)
                .HasMaxLength(100)
                .HasColumnName("District");

            addressBuilder.Property(a => a.Complement)
                .HasMaxLength(200)
                .HasColumnName("Complement");
        });
    }
}

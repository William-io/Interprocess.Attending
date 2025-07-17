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

        // Id é gerado manualmente no código
        builder.Property(p => p.Id)
            .ValueGeneratedNever(); // EF não gera o Id

        // Outros mapeamentos e configurações
        builder.Property(p => p.FirstName)
            .IsRequired()
            .HasMaxLength(100)
            .HasConversion(firstName => firstName.Value, firstName => new FirstName(firstName));

        builder.Property(p => p.LastName)
            .IsRequired()
            .HasMaxLength(100)
            .HasConversion(lastName => lastName.Value, lastName => new LastName(lastName));

        builder.Property(p => p.Cpf)
            .IsRequired()
            .HasMaxLength(11)
            .HasConversion(cpf => cpf.Value, cpf => new Document(cpf));

        builder.Property(p => p.DateBirth)
            .IsRequired()
            .HasMaxLength(10); // Formato "dd/MM/yyyy"

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

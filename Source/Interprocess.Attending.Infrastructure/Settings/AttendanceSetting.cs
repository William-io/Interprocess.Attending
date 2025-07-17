using Interprocess.Attending.Domain.Attendances;
using Interprocess.Attending.Domain.Clinics;
using Interprocess.Attending.Domain.Patients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Interprocess.Attending.Infrastructure.Settings;

internal sealed class AttendanceSetting : IEntityTypeConfiguration<Attendance>
{
    public void Configure(EntityTypeBuilder<Attendance> builder)
    {
        builder.ToTable("Attendances");

        builder.HasKey(attendance => attendance.Id);

        builder.Property(p => p.Status)
         .IsRequired()
         .HasConversion(
             status => status.ToString(),
             status => Enum.Parse<AttendanceStatus>(status));

        // Relacionamento Muitos para um onde um atendimento pertence a uma clínica
        // e uma clínica pode ter muitos atendimentos
        builder.HasOne<Clinic>()
            .WithMany()
            .HasForeignKey(booking => booking.ClinicId);

        // Relacionamento Muitos para um onde um paciente pode ter muitas atendimentos
        // e um atendimento pertence a um paciente
        builder.HasOne<Patient>()
            .WithMany()
            .HasForeignKey(booking => booking.PatientId);
    }
}
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

        builder.HasOne<Clinic>()
            .WithMany()
            .HasForeignKey(booking => booking.ClinicId);

        builder.HasOne<Patient>()
            .WithMany()
            .HasForeignKey(booking => booking.PatientId);
    }
}
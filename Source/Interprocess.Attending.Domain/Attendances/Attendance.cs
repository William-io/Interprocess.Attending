using Interprocess.Attending.Domain.Abstractions;
using Interprocess.Attending.Domain.Attendances.Events;
using Interprocess.Attending.Domain.Clinics;

namespace Interprocess.Attending.Domain.Attendances;

public sealed class Attendance : Entity
{
    /*
     * Construtor privado porque vai ser usando o factory method Create.
     */
    private Attendance(
        Guid id,
        Guid clinicId,
        Guid patientId,
        string description,
        AttendanceStatus status,
        DateTime createdOnUtc)
        : base(id)
    {
        ClinicId = clinicId;
        PatientId = patientId;
        Description = description;
        Status = status;
        CreatedOnUtc = createdOnUtc;
    }

    private Attendance()
    {
        
    }

    public Guid ClinicId { get; private set; }
    public Guid PatientId { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public DateTime CreatedOnUtc { get; private set; }
    public AttendanceStatus Status { get; private set; }

    public static Attendance MakeAnAttendanceRecord(
        Clinic clinic,
        Guid patientId,
        string description,
        DateTime createdOnUtc)
    {
        var attendance = new Attendance(
            Guid.NewGuid(),
            clinic.Id,
            patientId,
            description,
            AttendanceStatus.Active,
            createdOnUtc);

        // Validação para não permitir datas no futuro
        AttendanceDateValidator.ValidateAttendanceDateTime(createdOnUtc);

        attendance.AddDomainEvent(new AttendanceCreatedDomainEvent(attendance.Id));

        return attendance;
    }

    public Result Update(string description, DateTime createdOnUtc)
    {
        if (Status != AttendanceStatus.Active)
            return Result.Failure(AttendanceErrors.NotActive);

        // Validação para não permitir datas no futuro
        AttendanceDateValidator.ValidateAttendanceDateTime(createdOnUtc);

        Description = description;
        CreatedOnUtc = createdOnUtc;

        AddDomainEvent(new AttendanceUpdatedDomainEvent(Id));

        return Result.Success();
    }

    public Result Inactivate()
    {
        if (Status != AttendanceStatus.Active)
            return Result.Failure(AttendanceErrors.NotActive);

        Status = AttendanceStatus.Inactive;

        AddDomainEvent(new AttendanceInactivatedDomainEvent(Id));

        return Result.Success();
    }

}
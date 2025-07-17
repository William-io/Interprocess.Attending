namespace Interprocess.Attending.Application.Attendances.GetAttendancesByFilters;

public class AttendanceResponseFilter
{
    public Guid Id { get; init; }
    public Guid ClinicId { get; init; }
    public Guid PatientId { get; init; }
    public string Description { get; init; } = null!;
    public DateTime CreatedOnUtc { get; init; }
    public string Status { get; init; } = null!;
    public string PatientName { get; init; } = null!;
    public string ClinicName { get; init; } = null!;
    public string PatientCpf { get; init; } = null!;
    public string PatientDateBirth { get; init; } = null!;
}

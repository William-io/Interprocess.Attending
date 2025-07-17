namespace Interprocess.Attending.Application.Attendances.GetAttendance;

public class AttendanceResponse
{
    public Guid Id { get; init; }
    public Guid ClinicId { get; init; }
    public Guid PatientId { get; init; }
    public string Description { get; init; } = null!;
    public DateTime CreatedOnUtc { get; init; }
    public int Status { get; init; }
}
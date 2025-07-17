namespace Interprocess.Attending.API.Controllers.Attendances;

public sealed record CreateAttendanceRequest(
    Guid ClinicId,
    Guid PatientId,
    string Description,
    DateTime StartedDate);

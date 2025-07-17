namespace Interprocess.Attending.API.Controllers.Attendances;

public sealed record UpdateAttendanceRequest(
    string Description,
    DateTime CreatedOnUtc);

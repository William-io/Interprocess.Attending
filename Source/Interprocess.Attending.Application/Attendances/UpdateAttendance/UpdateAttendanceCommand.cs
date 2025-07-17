using Interprocess.Attending.Application.Abstractions.MessageCommunication;

namespace Interprocess.Attending.Application.Attendances.UpdateAttendance;

public sealed record UpdateAttendanceCommand(
    Guid AttendanceId,
    string Description,
    DateTime CreatedOnUtc) : ICommand;

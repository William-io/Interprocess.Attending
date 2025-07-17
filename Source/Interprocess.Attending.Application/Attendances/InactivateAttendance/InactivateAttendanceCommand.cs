using Interprocess.Attending.Application.Abstractions.MessageCommunication;

namespace Interprocess.Attending.Application.Attendances.InactivateAttendance;

public sealed record InactivateAttendanceCommand(Guid AttendanceId) : ICommand;

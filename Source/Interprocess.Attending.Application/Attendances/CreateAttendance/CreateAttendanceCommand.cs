using Interprocess.Attending.Application.Abstractions.MessageCommunication;

namespace Interprocess.Attending.Application.Attendances.CreateAttendance;

public sealed record CreateAttendanceCommand(
    Guid ClinicId,
    Guid PatientId,
    string Description,
    DateTime StartedDate) : ICommand<Guid>;
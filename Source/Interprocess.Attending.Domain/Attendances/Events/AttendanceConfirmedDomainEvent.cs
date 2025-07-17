using Interprocess.Attending.Domain.Abstractions;

namespace Interprocess.Attending.Domain.Attendances.Events;

public sealed record AttendanceConfirmedDomainEvent(Guid AttendanceId) : IDomainEvent;
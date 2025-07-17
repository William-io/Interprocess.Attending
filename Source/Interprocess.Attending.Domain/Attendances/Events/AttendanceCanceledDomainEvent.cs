using Interprocess.Attending.Domain.Abstractions;

namespace Interprocess.Attending.Domain.Attendances.Events;

public sealed record AttendanceCanceledDomainEvent(Guid AttendanceId) : IDomainEvent;
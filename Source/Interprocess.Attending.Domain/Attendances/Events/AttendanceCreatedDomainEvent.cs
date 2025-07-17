using Interprocess.Attending.Domain.Abstractions;

namespace Interprocess.Attending.Domain.Attendances.Events;

public sealed record AttendanceCreatedDomainEvent(Guid AttendanceId) : IDomainEvent;
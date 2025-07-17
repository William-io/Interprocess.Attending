using Interprocess.Attending.Domain.Abstractions;

namespace Interprocess.Attending.Domain.Patients.Events;

public sealed record PatientCreatedDomainEvent(Guid PatientId) : IDomainEvent;
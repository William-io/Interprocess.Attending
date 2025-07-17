using Interprocess.Attending.Application.Abstractions.MessageCommunication;
using Interprocess.Attending.Domain.Attendances;

namespace Interprocess.Attending.Application.Attendances.GetAttendancesByFilters;

public sealed record GetAttendancesByFiltersQuery(
    DateTime? StartDate = null,
    DateTime? EndDate = null,
    Guid? PatientId = null,
    AttendanceStatus? Status = null
) : IQuery<IEnumerable<AttendanceResponse>>;

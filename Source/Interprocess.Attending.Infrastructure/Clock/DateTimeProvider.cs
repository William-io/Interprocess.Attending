using Interprocess.Attending.Application.Abstractions.Clock;
using Interprocess.Attending.Domain.Attendances;

namespace Interprocess.Attending.Infrastructure.Clock;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow { get; }
}
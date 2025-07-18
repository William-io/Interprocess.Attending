using Interprocess.Attending.Application.Abstractions.Clock;

namespace Interprocess.Attending.Infrastructure.Clock;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
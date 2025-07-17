using Interprocess.Attending.Domain.Attendances;

namespace Interprocess.Attending.Application.Abstractions.Clock;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
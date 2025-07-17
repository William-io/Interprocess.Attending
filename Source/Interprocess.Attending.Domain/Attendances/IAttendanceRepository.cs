namespace Interprocess.Attending.Domain.Attendances;

public interface IAttendanceRepository
{
    Task<Attendance?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Attendance>> GetAllAsync(CancellationToken cancellationToken = default);
    void Add(Attendance attendance);
    void Update(Attendance attendance);
}
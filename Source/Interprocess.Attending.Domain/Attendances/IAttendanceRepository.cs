namespace Interprocess.Attending.Domain.Attendances;

public interface IAttendanceRepository
{
    Task<Attendance?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Attendance>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Attendance>> GetByFiltersAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        Guid? patientId = null,
        AttendanceStatus? status = null,
        CancellationToken cancellationToken = default);
    void Add(Attendance attendance);
    void Update(Attendance attendance);
}
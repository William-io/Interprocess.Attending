using Interprocess.Attending.Domain.Attendances;
using Microsoft.EntityFrameworkCore;

namespace Interprocess.Attending.Infrastructure.Repositories;

internal sealed class AttendanceRepository : IAttendanceRepository
{
    private readonly ApplicationDbContext _context;

    public AttendanceRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Attendance?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Attendance>()
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Attendance>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Set<Attendance>()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Attendance>> GetByFiltersAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        Guid? patientId = null,
        AttendanceStatus? status = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Set<Attendance>().AsQueryable();

        if (startDate.HasValue)
        {
            query = query.Where(a => a.CreatedOnUtc >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(a => a.CreatedOnUtc <= endDate.Value);
        }

        if (patientId.HasValue)
        {
            query = query.Where(a => a.PatientId == patientId.Value);
        }

        if (status.HasValue)
        {
            query = query.Where(a => a.Status == status.Value);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public void Add(Attendance attendance)
    {
        _context.Set<Attendance>().Add(attendance);
    }

    public void Update(Attendance attendance)
    {
        _context.Set<Attendance>().Update(attendance);
    }
}

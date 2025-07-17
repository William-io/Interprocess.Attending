using Interprocess.Attending.Domain.Patients;
using Microsoft.EntityFrameworkCore;

namespace Interprocess.Attending.Infrastructure.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly ApplicationDbContext _context;

    public PatientRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Patient?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Patient>()
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<Patient?> GetByCpfAsync(Document cpf, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Patient>()
            .FirstOrDefaultAsync(p => p.Cpf == cpf, cancellationToken);
    }

    public async Task<IEnumerable<Patient>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Set<Patient>()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Patient>> GetByFiltersAsync(
        string? name = null, 
        string? cpf = null, 
        PatientStatus? status = null, 
        CancellationToken cancellationToken = default)
    {
        var query = _context.Set<Patient>().AsQueryable();

        if (!string.IsNullOrEmpty(name))
        {
            query = query.Where(p => 
                p.FirstName!.Value.Contains(name) || 
                p.LastName!.Value.Contains(name));
        }

        if (!string.IsNullOrEmpty(cpf))
        {
            query = query.Where(p => p.Cpf.Value.Contains(cpf));
        }

        if (status.HasValue)
        {
            query = query.Where(p => p.Status == status.Value);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public void Update(Patient patient)
    {
        _context.Set<Patient>().Update(patient);
    }

    public void Add(Patient patient)
    {
        _context.Set<Patient>().Add(patient);
    }
}

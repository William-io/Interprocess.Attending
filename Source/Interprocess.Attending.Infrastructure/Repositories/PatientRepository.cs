using Interprocess.Attending.Domain.Patients;
using Microsoft.EntityFrameworkCore;

namespace Interprocess.Attending.Infrastructure.Repositories;

internal sealed class PatientRepository : Repository<Patient>, IPatientRepository
{
    private readonly ApplicationDbContext _context;

    public PatientRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public new async Task<Patient?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Patient>()
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<Patient?> GetByCpfAsync(string cpf, CancellationToken cancellationToken = default)
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

        if (!string.IsNullOrEmpty(cpf))
        {
            query = query.Where(p => p.Cpf.Contains(cpf));
        }

        if (status.HasValue)
        {
            query = query.Where(p => p.Status == status.Value);
        }

        var patients = await query.ToListAsync(cancellationToken);

        if (!string.IsNullOrEmpty(name))
        {
            patients = patients.Where(p => 
                p.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        return patients;
    }

    public void Update(Patient patient)
    {
        _context.Set<Patient>().Update(patient);
    }

    public new void Add(Patient patient)
    {
        _context.Set<Patient>().Add(patient);
    }
}

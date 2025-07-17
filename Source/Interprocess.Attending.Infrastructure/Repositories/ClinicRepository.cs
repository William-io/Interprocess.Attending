using Interprocess.Attending.Domain.Clinics;

namespace Interprocess.Attending.Infrastructure.Repositories;

internal sealed class ClinicRepository : Repository<Clinic>, IClinicRepository
{
    public ClinicRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
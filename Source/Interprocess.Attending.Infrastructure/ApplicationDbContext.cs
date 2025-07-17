using Interprocess.Attending.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Interprocess.Attending.Infrastructure;

public sealed class ApplicationDbContext : DbContext, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        
    }
}
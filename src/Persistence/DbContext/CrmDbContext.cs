using Application.Ports.DbContext;
using Application.Ports.Time;
using Domain.Abstractions;
using Domain.Aggregates.Account;
using Domain.Aggregates.Lead;
using Microsoft.EntityFrameworkCore;

namespace Persistence.DbContext;

public class CrmDbContext : Microsoft.EntityFrameworkCore.DbContext, ICrmContext
{
    private readonly IClock _clock;

    public CrmDbContext(DbContextOptions<CrmDbContext> options, IClock clock) : base(options)
    {
        _clock = clock;
    }

    public DbSet<Lead> Leads { get; set; }
    public DbSet<Account> Accounts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CrmDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            entry.Entity.UpdatedAt = _clock.UtcNow();
        }
    }
}
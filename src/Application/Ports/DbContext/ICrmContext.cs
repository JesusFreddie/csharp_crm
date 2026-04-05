using Domain.Aggregates.Account;
using Domain.Aggregates.Lead;
using Microsoft.EntityFrameworkCore;

namespace Application.Ports.DbContext;

public interface ICrmContext
{
    DbSet<Lead> Leads { get; set; }
    DbSet<Account> Accounts { get; set; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
}
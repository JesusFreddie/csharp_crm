using Application;
using Dapper;
using Domain.Entity;
using Domain.Repository;
using Row;

namespace Persistance.Repository;

public sealed class LeadRepository(IUnitToWork uow) : ILeadRepository
{
    public async Task Add(Lead lead, CancellationToken cancellationToken = default)
    {
        await uow.Connection.ExecuteAsync(
            """
            INSERT INTO leads (id, name, created_at, updated_at)
            VALUES (@id, @name, @created_at, @updated_at);
            """,
            lead,
            transaction: uow.Transaction);
    }

    public Task Update(Lead lead, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<Lead?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var row = await uow.Connection.QueryFirstOrDefaultAsync<LeadRow?>(
            """
            SELECT * FROM leads
            WHERE id = @id
            """, new { id }, uow.Transaction);
        if (!row.HasValue) return null;
        var lead = row.Value;
        return Lead.RestoreModel(
            id: lead.Id,
            name: lead.Name,
            description: lead.Description,
            isArchived: lead.IsArchived,
            createAt: lead.CreatedAt,
            updateAt: lead.UpdatedAt);
    }
}
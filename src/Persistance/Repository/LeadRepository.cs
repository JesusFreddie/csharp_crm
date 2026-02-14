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
            INSERT INTO leads (id, name)
            VALUES (@id, @name)
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
        var row = await uow.Connection.QueryFirstOrDefaultAsync<LeadRow>(
            """
            SELECT * FROM leads
            WHERE id = @id
            """, new { id }, uow.Transaction);

        return Lead.RestoreModel(
            id: row.Id,
            name: row.Name,
            description: row.Description,
            isArchived: row.IsArchived,
            createAt: row.CreatedAt,
            updateAt: row.UpdatedAt);
    }
}
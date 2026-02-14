using Domain.Entity;

namespace Domain.Repository;

public interface ILeadRepository
{
    public Task Add(Lead lead, CancellationToken cancellationToken = default);
    public Task Update(Lead lead, CancellationToken cancellationToken = default);
    public Task<Lead?> GetById(Guid id, CancellationToken cancellationToken = default);
}
using System.Data;

namespace Application;

public interface IUnitToWork : IDisposable
{
    IDbConnection Connection { get; }
    IDbTransaction? Transaction { get; }
    
    Task BeginAsync(CancellationToken cancellationToken = default);
    Task CommitAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync(CancellationToken cancellationToken = default);
}
using System.Data;
using Application;
using Npgsql;

namespace Persistance;

public sealed class UnitToWork(string connectionString) : IUnitToWork, IAsyncDisposable
{
    private readonly NpgsqlConnection _connection = new(connectionString);
    private NpgsqlTransaction? _transaction;
    
    public IDbConnection Connection => _connection;
    public IDbTransaction? Transaction => _transaction;

    public async Task BeginAsync(CancellationToken cancellationToken = default)
    {
        if (_connection.State != ConnectionState.Open)
            await _connection.OpenAsync(cancellationToken);
        _transaction = await _connection.BeginTransactionAsync(cancellationToken);
    }

    public Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
            throw new InvalidOperationException("Cannot commit a transaction while it is in a transaction");
        return _transaction.CommitAsync(cancellationToken);
    }

    public Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
            throw new InvalidOperationException("Cannot rollback a transaction while it is in a transaction");
        return _transaction.RollbackAsync(cancellationToken);
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _connection.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        if (_transaction != null) await _transaction.DisposeAsync();
        await _connection.DisposeAsync();
    }
}
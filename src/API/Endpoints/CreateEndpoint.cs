using CSharpFunctionalExtensions;
using Core.Error;

namespace API.Endpoints;

public class CreateEndpoint<TRequest, TResponse>
    : ResultEndpoint<TRequest, TResponse>
{
    protected async Task ExecuteAsync(
        Func<Task<Result<TResponse, BaseError>>> action,
        CancellationToken ct = default)
    {
        await base.ExecuteAsync(action, StatusCodes.Status201Created, ct);
    }
}
using Application.Error;
using CSharpFunctionalExtensions;
using Domain.Error;
using FastEndpoints;
using Core.Error;

namespace API.Endpoints;

public abstract class ResultEndpoint<TRequest, TResponse>
    : Endpoint<TRequest, TResponse>
{
    protected async Task ExecuteAsync(
        Func<Task<Result<TResponse, BaseError>>> action,
        int statusCode = StatusCodes.Status200OK,
        CancellationToken ct = default)
    {
        var result = await action();

        if (result.IsSuccess)
        {
            await Send.ResponseAsync(result.Value, statusCode, ct);
            return;
        }

        var status = ErrorHttpMapper.ToStatusCode(result.Error);
        ThrowError(result.Error.Message, result.Error.Code);
        ThrowIfAnyErrors(status);
    }
    
    protected async Task ExecuteAsync(
        Func<Task<IUnitResult<BaseError>>> action,
        CancellationToken ct = default)
    {
        var result = await action();

        if (result.IsSuccess)
        {
            await Send.NoContentAsync(ct);
            return;
        }

        var status = ErrorHttpMapper.ToStatusCode(result.Error);
        ThrowError(result.Error.Message, result.Error.Code);
        ThrowIfAnyErrors(status);
    }
}

public static class ErrorHttpMapper
{
    public static int ToStatusCode(BaseError error) =>
        error switch
        {
            DomainError => StatusCodes.Status400BadRequest,
            
            NotFound => StatusCodes.Status404NotFound,
            
            _ => StatusCodes.Status500InternalServerError
        };
}

public record ErrorResponse(
    string Code,
    string Message,
    IEnumerable<string>? Details = null
);
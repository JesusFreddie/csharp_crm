using Application.CQRS.Lead.Command.Archive;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace API.Endpoints.Lead.Archive;

[HttpPost("/api/lead/archive")]
public class Endpoint() : Endpoint<string, Results<Ok, NotFound>>
{
    public override Task<Results<Ok, NotFound>> ExecuteAsync(string req, CancellationToken ct)
    {
        return base.ExecuteAsync(req, ct);
    }
}
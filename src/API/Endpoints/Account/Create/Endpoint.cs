using Application.CQRS.Account.Command.Create;

namespace API.Endpoints.Account.Create;

public class Endpoint(Handler handle) : CreateEndpoint<Command, Domain.Aggregates.Account.Account>
{
    public override void Configure()
    {
        Post("api/account/create");
        AllowAnonymous();
        Description(c =>
        {
            c.Produces<Application.Entity.Lead>(StatusCodes.Status201Created);
            c.Produces<ErrorResponse>(StatusCodes.Status400BadRequest);
            c.Produces<ErrorResponse>(StatusCodes.Status404NotFound);
            c.Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);
        });
    }

    public override async Task HandleAsync(Command req, CancellationToken ct)
    {
        await ExecuteAsync(async () => await handle.Handle(req, ct), ct);
    }
}
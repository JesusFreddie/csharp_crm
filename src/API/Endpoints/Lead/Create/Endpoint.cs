using Application.CQRS.Lead.Command.Create;

namespace API.Endpoints.Lead.Create;

public class Endpoint(Handler handle) : CreateEndpoint<Command, Response>
{
    public override void Configure()
    {
        Post("api/lead/create");
        AllowAnonymous();
        Description(c =>
        {
            c.Produces<Response>(StatusCodes.Status201Created);
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
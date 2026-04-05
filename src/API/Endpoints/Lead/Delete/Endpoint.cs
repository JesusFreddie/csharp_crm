using Application.CQRS.Lead.Command.Delete;

namespace API.Endpoints.Lead.Delete;

public class Endpoint(Handler handler) : ResultEndpoint<Command, object>
{
    public override void Configure()
    {
        Delete("api/lead/{id:guid}");
        AllowAnonymous();

        Description(c =>
        {
            c.Produces(StatusCodes.Status204NoContent);
            c.Produces<ErrorResponse>(StatusCodes.Status404NotFound);
            c.Produces<ErrorResponse>(StatusCodes.Status400BadRequest);
            c.Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);
        });
    }

    public override async Task HandleAsync(Command req, CancellationToken ct)
    {
        await ExecuteAsync(async () => await handler.Handle(req, ct), ct);
    }
}

using Application.CQRS.Lead.Command.Archive;

namespace API.Endpoints.Lead.Archive;

public class Endpoint(Handler handler) : ResultEndpoint<Command, object>
{
    public override void Configure()
    {
        Post("api/lead/{id:guid}/archive");
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
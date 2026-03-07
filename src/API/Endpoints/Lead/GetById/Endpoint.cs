using Application.CQRS.Lead.Query.GetById;

namespace API.Endpoints.Lead.GetById;

public class Endpoint(Handler handler) : ResultEndpoint<Query, Application.Entity.Lead>
{
    public override void Configure()
    {
        Get("api/lead/{id:guid}");
        
        AllowAnonymous();
        
        Description(c =>
        {
            c.Produces<Query>(StatusCodes.Status200OK);
            c.Produces<ErrorResponse>(StatusCodes.Status404NotFound);
            c.Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);
        });
    }

    public override async Task HandleAsync(Query req, CancellationToken ct)
    {
        await ExecuteAsync(async () => await handler.Handle(req, ct), ct: ct);
    }
}
using Application.Error;
using Application.Ports.DbContext;
using Application.Ports.Handler;
using CSharpFunctionalExtensions;
using Core.Error;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Lead.Query.GetById;

public class Handler(ICrmContext context) : IQueryHandler
{
    public async Task<Result<Entity.Lead, BaseError>> Handle(Query query, CancellationToken cancellationToken)
    {
        var lead = await context.Leads.FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);
        if (lead is null) return new NotFound();

        return new Entity.Lead(lead.Id, lead.Name, lead.Description, lead.CreatedAt, lead.UpdatedAt);
    }
}
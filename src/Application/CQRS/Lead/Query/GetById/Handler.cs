using Application.Error;
using CSharpFunctionalExtensions;
using Domain.Repository;
using Shared;

namespace Application.CQRS.Lead.Query.GetById;

public class Handler(ILeadRepository leadRepository)
{
    public async Task<Result<Entity.Lead, BaseError>> Handle(Query query, CancellationToken cancellationToken)
    {
        var lead = await leadRepository.GetById(query.Id, cancellationToken);
        if (lead is null) return new NotFound();

        return new Entity.Lead(lead.Id, lead.Name, lead.Description, lead.CreatedAt, lead.UpdatedAt);
    }
}
using Application.Auth;
using Application.Error;
using Application.Policy.Lead;
using CSharpFunctionalExtensions;
using Domain.Repository;
using Shared;

namespace Application.CQRS.Lead.Query.GetById;

// public class Handler(ILeadRepository leadRepository, IGetLeadPolicy leadPolicy, ICurrentUser currentUser)
public class Handler(ILeadRepository leadRepository)
{
    public async Task<Result<Dto.Lead, BaseError>> Handle(Query query, CancellationToken cancellationToken)
    {
        // var can = await leadPolicy.CanExecute(currentUser, query, cancellationToken);
        // if (can.IsFailure) return can.Error;
        var lead = await leadRepository.GetById(query.Id, cancellationToken);
        if (lead is null) return new NotFound();

        return new Dto.Lead()
        {
            Id = lead.Id,
            Name = lead.Name,
            CreatedAt = lead.CreatedAt,
            UpdatedAt = lead.UpdatedAt
        };
    }
}
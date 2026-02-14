using Application.Auth;
using Application.Policy.Lead;
using CSharpFunctionalExtensions;
using Domain.Repository;
using Shared;

namespace Application.CQRS.Lead.Query.GetById;

public class Handler(ILeadRepository leadRepository, IGetLeadPolicy leadPolicy, ICurrentUser currentUser)
{
    public async Task<Result<Domain.Entity.Lead?, BaseError>> Handle(Query query, CancellationToken cancellationToken)
    {
        var can = await leadPolicy.CanExecute(currentUser, query, cancellationToken);
        if (can.IsFailure) return can.Error;
        return await leadRepository.GetById(query.Id, cancellationToken);
    }
}
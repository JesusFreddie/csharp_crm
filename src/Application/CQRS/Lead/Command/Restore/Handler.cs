using Application.Error;
using CSharpFunctionalExtensions;
using Domain.Repository;
using Shared;

namespace Application.CQRS.Lead.Command.Restore;

public class Handler(ILeadRepository leadRepository)
{
    public async Task<UnitResult<BaseError>> Handle(Command request, CancellationToken cancellationToken)
    {
        var lead = await leadRepository.GetById(request.Id, cancellationToken);
        if (lead is null) return new NotFound();

        var result = lead.Restore();
        if (result.IsFailure) return result.Error;

        return new UnitResult<BaseError>();
    }
}
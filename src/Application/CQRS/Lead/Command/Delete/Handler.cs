using Application.Error;
using CSharpFunctionalExtensions;
using Domain.Repository;
using Shared;

namespace Application.CQRS.Lead.Command.Delete;

public class Handler(ILeadRepository leadRepository)
{
    public async Task<UnitResult<BaseError>> Handle(Command cmd, CancellationToken cancellationToken)
    {
        var lead = await leadRepository.GetById(cmd.Id, cancellationToken);
        if (lead is null) return new NotFound();

        var result = lead.Delete();
        if (result.IsFailure) return result.Error;

        return new UnitResult<BaseError>();
    }
}
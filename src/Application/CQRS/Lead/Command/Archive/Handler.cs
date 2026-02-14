using Application.Error;
using CSharpFunctionalExtensions;
using Domain.Repository;
using Shared;

namespace Application.CQRS.Lead.Command.Archive;

public class Handler(ILeadRepository leadRepository, IUnitToWork uow)
{
    public async Task<UnitResult<BaseError>> Handle(Command cmd, CancellationToken ct)
    {
        await uow.BeginAsync(ct);

        try
        {
            var lead = await leadRepository.GetById(cmd.Id, ct);
            if (lead is null)
            {
                await uow.RollbackAsync(ct);
                return new NotFound();
            }

            var result = lead.Archive();
            if (result.IsFailure)
            {
                await uow.RollbackAsync(ct);
                return result.Error;
            }

            await leadRepository.Update(lead, ct);

            await uow.CommitAsync(ct);

            return UnitResult.Success<BaseError>();
        }
        catch
        {
            await uow.RollbackAsync(ct);
            throw;
        }
    }
}
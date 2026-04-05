using Application.Error;
using Application.Ports.DbContext;
using CSharpFunctionalExtensions;
using Core.Error;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Lead.Command.Archive;

public class Handler(ICrmContext context)
{
    public async Task<UnitResult<BaseError>> Handle(Command cmd, CancellationToken ct)
    {
        var lead = await context.Leads.FirstOrDefaultAsync(l => l.Id == cmd.Id, ct);
        if (lead is null)
            return new NotFound();

        var result = lead.Archive();
        if (result.IsFailure)
            return result.Error;

        await context.SaveChangesAsync(ct);
        return UnitResult.Success<BaseError>();
    }
}
using Application.Error;
using Application.Ports.DbContext;
using CSharpFunctionalExtensions;
using Core.Error;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Lead.Command.Delete;

public class Handler(ICrmContext context)
{
    public async Task<UnitResult<BaseError>> Handle(Command cmd, CancellationToken cancellationToken)
    {
        var lead = await context.Leads.FirstOrDefaultAsync(x => x.Id == cmd.Id, cancellationToken);
        if (lead is null) return new NotFound();

        var result = lead.Delete();
        if (result.IsFailure) return result.Error;

        await context.SaveChangesAsync(cancellationToken);

        return new UnitResult<BaseError>();
    }
}
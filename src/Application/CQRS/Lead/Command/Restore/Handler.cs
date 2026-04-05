using Application.Error;
using Application.Ports.DbContext;
using CSharpFunctionalExtensions;
using Core.Error;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Lead.Command.Restore;

public class Handler(ICrmContext context)
{
    public async Task<UnitResult<BaseError>> Handle(Command request, CancellationToken cancellationToken)
    {
        var lead = await context.Leads.FirstOrDefaultAsync(l => l.Id == request.Id, cancellationToken);
        if (lead is null) return new NotFound();

        var result = lead.Restore();
        if (result.IsFailure) return result.Error;

        await context.SaveChangesAsync(cancellationToken);

        return new UnitResult<BaseError>();
    }
}
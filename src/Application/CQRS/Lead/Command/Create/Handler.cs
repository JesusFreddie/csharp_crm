using CSharpFunctionalExtensions;
using Domain.Repository;
using Shared;

namespace Application.CQRS.Lead.Command.Create;

public sealed class Handler(ILeadRepository leadRepository)
{
    public async Task<Result<Entity.Lead, BaseError>> Handle(Command cmd, CancellationToken cancellationToken)
    {
        // var can = await canLeadPolicy.CanExecute(currentUser, cmd, cancellationToken);
        // if (can.IsFailure)
        // {
        //     return can.Error;
        // }
        
        var id = Guid.NewGuid();
        var date = DateTime.UtcNow;
        var leadResult = Domain.Entity.Lead.Create(id, cmd.Name, cmd.Description, date, date);
        
        if (leadResult.IsFailure)
            return leadResult.Error;
        
        var lead = leadResult.Value;
        await leadRepository.Add(lead, cancellationToken);
        
        return new Entity.Lead(lead.Id, lead.Name, lead.Description, lead.CreatedAt, lead.UpdatedAt);
    }
}
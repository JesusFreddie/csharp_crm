using Application.Auth;
using Application.Policy.Lead;
using CSharpFunctionalExtensions;
using Domain.Repository;
using Shared;

namespace Application.CQRS.Lead.Command.Create;

// public sealed class Handler(ILeadRepository leadRepository, ICreateLeadPolicy canLeadPolicy, ICurrentUser currentUser)
public sealed class Handler(ILeadRepository leadRepository)
{
    public async Task<Result<Response, BaseError>> Handle(Command cmd, CancellationToken cancellationToken)
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
        
        return new Response(lead.Id, lead.Name, lead.Description, lead.CreatedAt, lead.UpdatedAt);
    }
}
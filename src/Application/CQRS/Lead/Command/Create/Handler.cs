using Application.Ports.Id;
using Application.Ports.Messaging;
using Application.Ports.DbContext;
using Application.Ports.Time;
using Contracts.Messages.Lead;
using CSharpFunctionalExtensions;
using Core.Error;
using Domain.Entity;

namespace Application.CQRS.Lead.Command.Create;

public sealed class Handler(
    ICrmContext context,
    IClock clock,
    IIdGenerator idGenerator,
    IMessagePublisher messagePublisher)
{
    public async Task<Result<Entity.Lead, BaseError>> Handle(Command cmd, CancellationToken cancellationToken)
    {
        var date = clock.UtcNow();
        var amount = DealAmount.Empty();

        var result = Domain.Entity.Lead.Create(
            id: idGenerator.New(),
            name: cmd.Name,
            description: cmd.Description,
            amount: amount,
            createAt: date,
            updateAt: date);

        if (result.IsFailure)
            return result.Error;

        var lead = result.Value;

        await context.Leads.AddAsync(lead, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        var leadCreatedEvent = new LeadCreatedEvent
        {
            LeadId = lead.Id,
            Name = lead.Name,
            Description = lead.Description,
            Amount = lead.Amount.Amount,
            PricingMode = lead.Amount.Mode.ToString(),
            CreatedAt = lead.CreatedAt
        };

        await messagePublisher.PublishAsync(leadCreatedEvent, cancellationToken);

        return new Entity.Lead(lead.Id, lead.Name, lead.Description, lead.CreatedAt, lead.UpdatedAt);
    }
}
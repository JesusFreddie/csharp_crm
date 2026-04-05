using Application.Ports.DbContext;
using Application.Ports.Handler;
using Application.Ports.Id;
using Application.Ports.Messaging;
using Application.Ports.Time;
using Contracts.Messages.Account;
using Core.Error;
using CSharpFunctionalExtensions;

namespace Application.CQRS.Account.Command.Create;

public class Handler(ICrmContext context, IIdGenerator generator, IClock clock, IMessagePublisher publisher)
    : ICommandHandler
{
    public async Task<Result<Domain.Aggregates.Account.Account, BaseError>> Handle(Command cmd,
        CancellationToken cancellationToken)
    {
        var result = Domain.Aggregates.Account.Account.Create(id: generator.New(), name: cmd.Name,
            createdAt: clock.UtcNow(),
            updatedAt: clock.UtcNow());
        if (result.IsFailure)
            return result.Error;

        await context.Accounts.AddAsync(result.Value, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        await publisher.PublishAsync(new AccountCreatedEvent
        {
            AccountId = result.Value.Id,
            Name = result.Value.Name,
            CreatedAt = result.Value.CreatedAt,
        }, cancellationToken);

        return result.Value;
    }
}
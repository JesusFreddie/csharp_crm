namespace Contracts.Messages.Account;

public class AccountCreatedEvent
{
    public Guid AccountId { get; init; }
    public string Name { get; init; }
    public DateTime CreatedAt { get; init; }
}
namespace Contracts.Messages.Lead;

public record LeadCreatedEvent
{
    public Guid LeadId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public decimal Amount { get; init; }
    public string PricingMode { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}

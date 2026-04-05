namespace Domain.Entity;

public abstract class BaseEntity
{
    public Guid Id { get; protected init; }
    public DateTime CreatedAt { get; protected init; }
    public DateTime UpdatedAt { get; internal set; }
}
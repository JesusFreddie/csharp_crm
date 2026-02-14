namespace Row;

internal readonly record struct LeadRow
{
    public Guid Id {get; init;}
    public bool IsArchived  {get; init;}
    public string Name { get; init; }
    public string Description { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}
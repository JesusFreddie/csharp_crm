using System.Text.Json.Serialization;

namespace Application.CQRS.Lead.Command.Create;

public record Response(Guid Id, string Name, string Description, DateTime CreatedAt, DateTime UpdatedAt);
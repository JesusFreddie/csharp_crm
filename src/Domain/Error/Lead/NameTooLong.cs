namespace Domain.Error.Lead;

public record NameTooLong() : LeadError("lead.name.too_long", "Lead name too long");
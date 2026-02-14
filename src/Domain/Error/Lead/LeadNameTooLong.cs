namespace Domain.Error.Lead;

public record LeadNameTooLong() : LeadError("lead.name.too_long", "Lead name too long");
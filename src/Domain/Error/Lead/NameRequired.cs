namespace Domain.Error.Lead;

public record NameRequired() : LeadError("lead.name.required", "Lead name is required");
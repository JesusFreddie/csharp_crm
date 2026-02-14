namespace Domain.Error.Lead;

public record LeadNameRequired() : LeadError("lead.name.required", "Lead name is required");
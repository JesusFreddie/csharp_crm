namespace Domain.Error.Lead;

public record DescriptionTooLong() : LeadError("lead.description.too_long", "Description too long");
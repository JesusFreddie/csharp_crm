namespace Domain.Error.Lead;

public record CannotModifyArchived() : LeadError("lead.cannot_modify_archived", "CannotModifyArchived");
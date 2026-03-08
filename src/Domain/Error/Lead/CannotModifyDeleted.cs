namespace Domain.Error.Lead;

public record CannotModifyDeleted() : LeadError("lead.cannot_modified_deleted", "CannotModifyDeleted");
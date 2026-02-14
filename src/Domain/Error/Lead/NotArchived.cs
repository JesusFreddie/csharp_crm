namespace Domain.Error.Lead;

public record NotArchived() : DomainError("lead.not_archived", "Lead Not Archived");

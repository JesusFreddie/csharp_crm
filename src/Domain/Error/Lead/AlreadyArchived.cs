namespace Domain.Error.Lead;

public record AlreadyArchived() : LeadError("lead.already_archived", "Lead already archived");
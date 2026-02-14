namespace Domain.Error.Lead;

public sealed record AlreadyDeleted() : LeadError("lead.already_deleted", "Lead already deleted");
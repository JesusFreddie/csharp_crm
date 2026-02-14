using CSharpFunctionalExtensions;

namespace Domain.Entity;

public class LeadAccountLink : BaseEntity
{
    public Guid AccountId { get; set; }
    public Guid LeadId { get; set; }

    private LeadAccountLink(Guid accountId, Guid leadId)
    {
        AccountId = accountId;
        LeadId = leadId;
    }

    public static Result<LeadAccountLink> Create(
        Guid accountId,
        Guid leadId
    )
    {
        return new LeadAccountLink(accountId, leadId);
    }
}
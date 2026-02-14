using CSharpFunctionalExtensions;
using Domain.Error;
using Domain.Error.Lead;
using Shared;

namespace Domain.Entity;

public class Lead : BaseEntity, IArchivable
{
    const int MaxNameLength = 128;

    public string Name { get; }
    public string Description { get; }
    
    public bool IsArchived { get; private set; } = false;
    public bool IsDeleted { get; private set; } = false;

    public static Result<Lead, LeadError> Create(Guid id, string name, string description, DateTime createAt, DateTime updateAt)
    {
        if (string.IsNullOrWhiteSpace(name)) return new LeadNameRequired();
        if (name.Length > MaxNameLength) return new LeadNameTooLong();

        return new Lead(id, name, description, createAt, updateAt);
    }

    public UnitResult<DomainError> Archive()
    {
        if (IsArchived) return new AlreadyArchived();

        IsArchived = true;

        return UnitResult.Success<DomainError>();
    }

    public UnitResult<DomainError> Restore()
    {
        if (!IsArchived) return new NotArchived();

        IsArchived = false;
        return UnitResult.Success<DomainError>();
    }

    public UnitResult<DomainError> Delete()
    {
        if (IsDeleted) return new AlreadyDeleted();
        IsDeleted = true;
        return UnitResult.Success<DomainError>();
    }
    
    private Lead(Guid id, string name, string description, DateTime createAt, DateTime updateAt)
    {
        Name = name;
        Id = id;
        Description = description;
        CreatedAt = createAt;
        UpdatedAt = updateAt;
    }

    public static Lead RestoreModel(
        Guid id,
        string name,
        string description,
        bool isArchived,
        DateTime createAt,
        DateTime updateAt)
    {
        var lead = new Lead(id, name, description, createAt, updateAt)
        {
            IsArchived = isArchived
        };
        return lead;
    }
}
using CSharpFunctionalExtensions;
using Domain.Error;
using Domain.Error.Lead;
using Shared;

namespace Domain.Entity;

public class Lead : BaseEntity, IArchivable
{
    const int MaxNameLength = 128;
    const int MaxDescriptionLength = 1000;

    public string Name { get; private set;  }
    public string Description { get; private set; }
    
    public bool IsArchived { get; private set; } = false;
    public bool IsDeleted { get; private set; } = false;

    public static Result<Lead, LeadError> Create(Guid id, string name, string description, DateTime createAt, DateTime updateAt)
    {
        name = name.Trim();
        description = description?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(name)) 
            return new NameRequired();
        if (name.Length > MaxNameLength) 
            return new NameTooLong();
        if (description.Length > MaxDescriptionLength) 
            return new DescriptionTooLong();
        
        return new Lead(id, name, description, createAt, updateAt);
    }


    public UnitResult<DomainError> Archive()
    {
        if (IsDeleted) 
            return new CannotModifyDeleted();
        if (IsArchived) 
            return new AlreadyArchived();

        IsArchived = true;
        UpdatedAt = DateTime.UtcNow;
        return UnitResult.Success<DomainError>();
    }

    public UnitResult<DomainError> Restore()
    {
        if (IsDeleted) 
            return new CannotModifyDeleted();
        if (!IsArchived) 
            return new NotArchived();

        IsArchived = false;
        UpdatedAt = DateTime.UtcNow;
        return UnitResult.Success<DomainError>();
    }

    public UnitResult<DomainError> Delete()
    {
        if (IsDeleted) 
            return new AlreadyDeleted();
        
        IsDeleted = true;
        UpdatedAt = DateTime.UtcNow;
        return UnitResult.Success<DomainError>();
    }

    public UnitResult<DomainError> SetName(string name)
    {
        if (IsDeleted) 
            return new CannotModifyDeleted();
        if (IsArchived) 
            return new CannotModifyArchived();

        name = name.Trim();
        if (string.IsNullOrWhiteSpace(name)) 
            return new NameRequired();
        if (name.Length > MaxNameLength) 
            return new NameTooLong();

        Name = name;
        UpdatedAt = DateTime.UtcNow;
        return UnitResult.Success<DomainError>();
    }
    
    public UnitResult<DomainError> SetDescription(string description)
    {
        if (IsDeleted) 
            return new CannotModifyDeleted();
        if (IsArchived) 
            return new CannotModifyArchived();

        description = description?.Trim() ?? string.Empty;
        if (description.Length > MaxDescriptionLength) 
            return new DescriptionTooLong();

        Description = description;
        UpdatedAt = DateTime.UtcNow;
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
    
    public override bool Equals(object? obj)
    {
        if (obj is not Lead other) return false;
        return Id == other.Id;
    }

    public override int GetHashCode() => Id.GetHashCode();
}
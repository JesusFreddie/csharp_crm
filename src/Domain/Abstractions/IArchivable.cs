using CSharpFunctionalExtensions;
using Domain.Error;

namespace Domain.Abstractions;

public interface IArchivable
{
    bool IsArchived { get; }

    public UnitResult<DomainError> Archive();
    public UnitResult<DomainError> Restore();
}
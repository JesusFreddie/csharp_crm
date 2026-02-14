using Application.Auth;
using Application.CQRS.Lead.Command.Create;
using CSharpFunctionalExtensions;
using Shared;

namespace Application.Policy.Lead;

public interface ICreateLeadPolicy
{
    public Task<UnitResult<BaseError>> CanExecute(ICurrentUser userId, Command cmd, CancellationToken ct = default);
}
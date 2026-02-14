using Application.CQRS.Lead.Command.Create;
using FastEndpoints;
using FluentValidation;

namespace API.Endpoints.Lead.Create;

public sealed class Validator : Validator<Command>
{
    public Validator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(5000).WithMessage("Name cannot exceed 5000 characters");
    }
}
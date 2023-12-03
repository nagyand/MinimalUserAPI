using FluentValidation;
using MinimalUserAPI.Application.Entity;

namespace MinimalUserAPI.Application.Validations;
public class CompanyValidator : AbstractValidator<Company>
{
    public CompanyValidator()
    {
        RuleFor(s => s.Name).NotEmpty();
        RuleFor(s => s.CatchPhrase).NotEmpty();
        RuleFor(s => s.Bs).NotEmpty();
    }
}

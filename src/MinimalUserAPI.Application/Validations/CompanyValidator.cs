using FluentValidation;
using MinimalUserAPI.Application.Entity;

namespace MinimalUserAPI.Application.Validations;
public class CompanyValidator : AbstractValidator<Company>
{
    public CompanyValidator()
    {
        RuleFor(s => s.Name).NotEmpty().Matches(@"^([a-zA-Z\s\-]+)$");
        RuleFor(s => s.CatchPhrase).NotEmpty().Matches(@"^([a-zA-Z\s\-]+)$");
        RuleFor(s => s.Bs).NotEmpty().Matches(@"^([a-zA-Z\s\-]+)$");
    }
}

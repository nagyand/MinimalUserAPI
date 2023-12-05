using FluentValidation;
using MinimalUserAPI.Application.Entity;

namespace MinimalUserAPI.Application.Validations;
public class CompanyValidator : AbstractValidator<Company>
{
    private readonly string companyValidationRegex = @"^([a-zA-Z\s\-]+)$";
    public CompanyValidator()
    {
        RuleFor(s => s.Name).NotEmpty().Matches(companyValidationRegex);
        RuleFor(s => s.CatchPhrase).NotEmpty().Matches(companyValidationRegex);
        RuleFor(s => s.Bs).NotEmpty().Matches(companyValidationRegex);
    }
}

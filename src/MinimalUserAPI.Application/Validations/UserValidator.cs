using FluentValidation;
using MinimalUserAPI.Application.Entity;

namespace MinimalUserAPI.Application.Validations;
public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(s => s.Id).NotEmpty().GreaterThan(0);
        RuleFor(s => s.Name).NotEmpty().Matches(@"([a-zA-Z.]{2,3})?\s?[a-zA-Z]+\s[a-zA-Z]+");
        RuleFor(s => s.UserName).NotEmpty().Matches(@"^([a-zA-Z._\s]+)$");
        RuleFor(s => s.Email).NotEmpty().EmailAddress();
        RuleFor(s => s.Phone).NotEmpty();
        RuleFor(s => s.Website).NotEmpty().Matches(@"[a-z]+\.[a-z]{2,}");
        RuleFor(s => s.Address).NotNull().SetValidator(new AddressValidator());
        RuleFor(s => s.Company).NotNull().SetValidator(new CompanyValidator());
    }
}

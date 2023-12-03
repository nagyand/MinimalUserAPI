using FluentValidation;
using MinimalUserAPI.Application.Entity;

namespace MinimalUserAPI.Application.Validations;
public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(s => s.Id).NotEmpty().GreaterThan(0);
        RuleFor(s => s.Name).NotEmpty();
        RuleFor(s => s.UserName).NotEmpty();
        RuleFor(s => s.Email).NotEmpty().EmailAddress();
        RuleFor(s => s.Phone).NotEmpty();
        RuleFor(s => s.Website).NotEmpty();
        RuleFor(s => s.Address).NotNull().SetValidator(new AddressValidator());
        RuleFor(s => s.Company).NotNull().SetValidator(new CompanyValidator());
    }
}

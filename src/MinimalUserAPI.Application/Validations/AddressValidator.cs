using FluentValidation;
using MinimalUserAPI.Application.Entity;

namespace MinimalUserAPI.Application.Validations;
public class AddressValidator : AbstractValidator<Address>
{
    public AddressValidator()
    {
        RuleFor(s => s.Street).NotNull();
        RuleFor(s => s.City).NotEmpty();
        RuleFor(s => s.Suite).NotEmpty();
        RuleFor(s => s.ZipCode).NotEmpty();
        RuleFor(s => s.Geo).NotNull().SetValidator(new GeoValidator());
    }
}

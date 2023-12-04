using FluentValidation;
using MinimalUserAPI.Application.Entity;

namespace MinimalUserAPI.Application.Validations;
public class AddressValidator : AbstractValidator<Address>
{
    public AddressValidator()
    {
        RuleFor(s => s.Street).NotNull().Matches(@"^([a-zA-Z\s]+)$");
        RuleFor(s => s.City).NotEmpty().Matches(@"^([a-zA-Z]+)$");
        RuleFor(s => s.Suite).NotEmpty().Matches(@"^((Suite|Apt.)+ \d+)$");
        RuleFor(s => s.ZipCode).NotEmpty().Matches(@"^(\d{5}-\d{4})$|^(\d{5})$");
        RuleFor(s => s.Geo).NotNull().SetValidator(new GeoValidator());
    }
}

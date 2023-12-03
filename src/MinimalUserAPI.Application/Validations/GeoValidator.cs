using FluentValidation;
using MinimalUserAPI.Application.Entity;

namespace MinimalUserAPI.Application.Validations;
public class GeoValidator : AbstractValidator<Geo>
{
    public GeoValidator()
    {
        RuleFor(s => s.Lng).NotNull();
        RuleFor(s => s.Lat).NotNull();
    }
}

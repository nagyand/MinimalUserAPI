﻿using FluentValidation;
using MinimalUserAPI.Application.Entity;

namespace MinimalUserAPI.Application.Validations;
public class GeoValidator : AbstractValidator<Geo>
{
    public GeoValidator()
    {
        RuleFor(s => s.Lng).NotNull().InclusiveBetween(-180, 180);
        RuleFor(s => s.Lat).NotNull().InclusiveBetween(-90, 90);
    }
}

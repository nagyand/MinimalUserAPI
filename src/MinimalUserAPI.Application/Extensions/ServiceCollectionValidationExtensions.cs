using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MinimalUserAPI.Application.Entity;
using MinimalUserAPI.Application.Validations;

namespace MinimalUserAPI.Application.Extensions;
public static class ServiceCollectionValidationExtensions
{
    public static void ConfigureValidations(this IServiceCollection services)
    {
        services.AddScoped<IValidator<User>, UserValidator>();
        services.AddScoped<IValidator<Company>, CompanyValidator>();
        services.AddScoped<IValidator<Address>, AddressValidator>();
        services.AddScoped<IValidator<Geo>, GeoValidator>();
    }
}

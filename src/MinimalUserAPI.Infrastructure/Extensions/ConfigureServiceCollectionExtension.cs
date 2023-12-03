using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MinimalUserAPI.Application.Interfaces;
using MinimalUserAPI.Infrastructure.Configurations;
using MinimalUserAPI.Infrastructure.DbContext;
using MinimalUserAPI.Infrastructure.Users;
using MinimalUserAPI.Infrastructure.Validations;

namespace MinimalUserAPI.Infrastructure.Extensions;
public static class ConfigureServiceCollectionExtension
{
    public static void ConfigureInfrastructure(this IServiceCollection serviceCollection, IConfiguration config)
    {
        serviceCollection.AddSingleton<IValidateOptions<UserDbContextConfiguration>, UserDbContextConfigurationValidation>();
        serviceCollection.AddOptions<UserDbContextConfiguration>()
                         .Bind(config.GetRequiredSection(UserDbContextConfiguration.ConfigurationRootName))
                         .ValidateOnStart();
        serviceCollection.AddScoped<UserDbContext>();
        serviceCollection.AddScoped<IUserRepository, UserRepository>();
    }
}

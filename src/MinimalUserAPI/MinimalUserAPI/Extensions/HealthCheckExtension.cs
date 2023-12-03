using MinimalUserAPI.Infrastructure.Configurations;

namespace MinimalUserAPI.Extensions;

public static class HealthCheckExtension
{
    public static void ConfiguraMongoDbHealthCheck(this IServiceCollection services, IConfiguration config)
    {
        UserDbContextConfiguration mongoConfig = new();
        config.GetRequiredSection(UserDbContextConfiguration.ConfigurationRootName).Bind(mongoConfig);
        services.AddHealthChecks().AddMongoDb(mongoConfig.ConnectionString);
    }
}

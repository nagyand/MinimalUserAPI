namespace MinimalUserAPI.Infrastructure.Configurations;
public record UserDbContextConfiguration
{
    public static string ConfigurationRootName = "UserDbContextConfiguration";
    public string ConnectionString { get; init; }
    public string DatabaseName { get; init; }
}

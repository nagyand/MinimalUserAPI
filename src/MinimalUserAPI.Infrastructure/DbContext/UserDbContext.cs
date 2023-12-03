using Microsoft.Extensions.Options;
using MinimalUserAPI.Application.Entity;
using MinimalUserAPI.Infrastructure.Configurations;
using MongoDB.Driver;

namespace MinimalUserAPI.Infrastructure.DbContext;
public class UserDbContext
{
    private readonly IMongoDatabase database;
    private readonly UserDbContextConfiguration configuration;
    public UserDbContext(IOptions<UserDbContextConfiguration> configuration)
    {
        ArgumentNullException.ThrowIfNull(nameof(configuration));
        this.configuration = configuration.Value;
        var client = new MongoClient(configuration.Value.ConnectionString);
        database = client.GetDatabase(configuration.Value.DatabaseName);
    }
    public IMongoCollection<User> Users => database.GetCollection<User>(configuration.CollectionName);
}

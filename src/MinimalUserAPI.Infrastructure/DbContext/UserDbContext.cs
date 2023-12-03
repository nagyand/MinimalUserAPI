using Microsoft.Extensions.Options;
using MinimalUserAPI.Application.Entity;
using MinimalUserAPI.Infrastructure.Configurations;
using MongoDB.Driver;

namespace MinimalUserAPI.Infrastructure.DbContext;
public class UserDbContext
{
    private readonly IMongoDatabase _database;
    public UserDbContext(IOptions<UserDbContextConfiguration> configuration)
    {
        ArgumentNullException.ThrowIfNull(nameof(configuration));
        var client = new MongoClient(configuration.Value.ConnectionString);
        _database = client.GetDatabase(configuration.Value.DatabaseName);
    }
    public IMongoCollection<User> Users => _database.GetCollection<User>("users");
}

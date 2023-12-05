using MinimalUserAPI.Application.Entity;
using MinimalUserAPI.Application.Interfaces;
using MinimalUserAPI.Infrastructure.DbContext;
using MongoDB.Driver;

namespace MinimalUserAPI.Infrastructure.Users;
public class UserRepository : IUserRepository
{
    private readonly UserDbContext dbContext;
    public UserRepository(UserDbContext dbContext)
    {
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async ValueTask<long> DeleteUser(int userId)
    {
        if (userId <= 0)
        {
            throw new ArgumentException($"{nameof(userId)} must be greater than '0'");
        }
        var deleteResult = await dbContext.Users.DeleteOneAsync(s => s.Id == userId);
        return deleteResult.DeletedCount;
    }

    public async ValueTask<IEnumerable<User>> GetUsersByFilter(FilterDefinition<User> filter)
    {
        ArgumentNullException.ThrowIfNull(filter, nameof(filter));
        var users = await dbContext.Users.FindAsync(filter);
        return await users.ToListAsync();
    }

    public async ValueTask<User> InsertUser(User user)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user)); 
        await dbContext.Users.InsertOneAsync(user);
        return user;
    }

    public Task<User> UpdateUser(int userId,User user)
    {
        if (userId <= 0)
        {
            throw new ArgumentException($"{nameof(userId)} must be greater than '0'");
        }
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        return dbContext.Users.FindOneAndReplaceAsync(s => s.Id == userId, user);
    }
}

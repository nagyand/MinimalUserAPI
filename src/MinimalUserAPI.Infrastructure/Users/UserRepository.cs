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
        var deleteResult = await dbContext.Users.DeleteOneAsync(s => s.Id == userId);
        return deleteResult.DeletedCount;
    }

    public async ValueTask<IEnumerable<User>> GetUsers()
    {
        var filter = Builders<User>.Filter.Empty;
        var users = await dbContext.Users.FindAsync(filter);
        return await users.ToListAsync();
    }

    public async ValueTask<User> InsertUser(User user)
    {
        await dbContext.Users.InsertOneAsync(user);
        return user;

    }

    public async ValueTask<User> UpdateUser(int userId,User user)
    {
        var replaceResult = await dbContext.Users.ReplaceOneAsync(s => s.Id == userId, user);
        if (replaceResult.ModifiedCount == 0)
        {
            await dbContext.Users.InsertOneAsync(user);
        }
        return user;
    }
}

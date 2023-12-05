using Microsoft.Extensions.Logging;
using MinimalUserAPI.Application.Entity;
using MinimalUserAPI.Application.Interfaces;
using MongoDB.Driver;

namespace MinimalUserAPI.Application.Users.Services;
public class UserService : IUserService
{
    private readonly IUserRepository userRepository;
    private readonly ILogger<UserService>? logger;
    public UserService(IUserRepository userRepository, ILogger<UserService>? logger = null)
    {
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        this.logger = logger;
    }
    public ValueTask<long> DeleteUser(int userId)
    {
        if (userId < 0)
        {
            throw new ArgumentException("User Id must be greater than '0'");
        }
        logger?.LogInformation("Delete user with id '{userId}'", userId);
        return userRepository.DeleteUser(userId);
    }

    public ValueTask<IEnumerable<User>> GetUsers()
    {
        logger?.LogInformation("Get all users");
        FilterDefinition<User> filter = Builders<User>.Filter.Empty;
        return userRepository.GetUsersByFilter(filter);
    }

    public ValueTask<User> InsertUser(User user)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        logger?.LogInformation("Create new user with id '{id}'", user.Id);
        return userRepository.InsertUser(user);
    }

    public async ValueTask<User> UpdateUser(int userId, User user)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        logger?.LogInformation("Update user with id '{userId}'", userId);
        var updatedUser = await userRepository.UpdateUser(userId, user);
        if (updatedUser is null)
        {
            logger?.LogInformation("User is not exists with user id '{id}'. Create a new one", userId);
            return await InsertUser(user);
        }
        logger?.LogInformation("User is updated");
        return user;
    }
}

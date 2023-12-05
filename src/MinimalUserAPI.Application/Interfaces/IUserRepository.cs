using MinimalUserAPI.Application.Entity;
using MongoDB.Driver;

namespace MinimalUserAPI.Application.Interfaces;
public interface IUserRepository
{
    /// <summary>
    /// Get one or more users by filter
    /// </summary>
    /// <param name="filter">user filter</param>
    /// <returns>list of users</returns>
    ValueTask<IEnumerable<User>> GetUsersByFilter(FilterDefinition<User> filter);
    /// <summary>
    /// Delete user by user id
    /// </summary>
    /// <param name="userId">user id</param>
    /// <returns>number of user deleted</returns>
    ValueTask<long> DeleteUser(int userId);
    /// <summary>
    /// Update or create user
    /// </summary>
    /// <param name="userId">user id</param>
    /// <param name="user">Updated user information</param>
    /// <returns>Updated user information</returns>
    Task<User> UpdateUser(int userId, User user);
    /// <summary>
    /// Create new user
    /// </summary>
    /// <param name="user">new user to create</param>
    /// <returns>created user</returns>
    ValueTask<User> InsertUser(User user);
}

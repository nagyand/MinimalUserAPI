using MinimalUserAPI.Application.Entity;

namespace MinimalUserAPI.Application.Interfaces;

public interface IUserService
{
    /// <summary>
    /// Get all users
    /// </summary>
    /// <returns></returns>
    ValueTask<IEnumerable<User>> GetUsers();
    /// <summary>
    /// Delete user by user id.
    /// </summary>
    /// <param name="userId">User's id to delete</param>
    /// <returns>It return number of user deleted. If user is not found then returns with zero</returns>
    ValueTask<long> DeleteUser(int userId);
    /// <summary>
    /// Update user's data. If user is not exists then it creates a new one.
    /// </summary>
    /// <param name="userId">User's id to update</param>
    /// <param name="user">User model</param>
    /// <returns>Updated user model</returns>
    ValueTask<User> UpdateUser(int userId, User user);
    /// <summary>
    /// Create new user
    /// </summary>
    /// <param name="user">new user to create</param>
    /// <returns>Created user</returns>
    ValueTask<User> InsertUser(User user);
}

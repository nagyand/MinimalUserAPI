using MinimalUserAPI.Application.Entity;

namespace MinimalUserAPI.Application.Interfaces;
public interface IUserRepository
{
    public ValueTask<IEnumerable<User>> GetUsers();
    public ValueTask<long> DeleteUser(int userId);
    public ValueTask<User> UpdateUser(int userId, User user);
    public ValueTask<User> InsertUser(User user);
}

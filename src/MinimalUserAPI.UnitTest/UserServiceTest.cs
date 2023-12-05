using MinimalUserAPI.Application.Entity;
using MinimalUserAPI.Application.Interfaces;
using MinimalUserAPI.Application.Users.Services;
using MinimalUserAPI.UnitTest.TestData;
using NSubstitute;

namespace MinimalUserAPI.UnitTest;
public class UserServiceTest
{
    [Fact]
    public void UserService_UserRepositoryNull_Test()
    {
        //Arrange / Act / Assert
        Assert.Throws<ArgumentNullException>(() => new UserService(null!));
    }

    [Fact]
    public async Task UserService_GetUsers_Test()
    {
        //Arrange
        IUserRepository userRepository = Substitute.For<IUserRepository>();
        var users = UserTestData.GetUsers();
        userRepository.GetUsersByFilter(default!).ReturnsForAnyArgs(users);
        UserService userService = new(userRepository);

        //Act
        var allUsers = await userService.GetUsers();

        //Assert
        Assert.Equal(users, allUsers);
    }

    [Fact]
    public async Task UserService_InsertUser_NullUser_Test()
    {
        //Arrange
        IUserRepository userRepository = Substitute.For<IUserRepository>();
        UserService userService = new(userRepository);

        //Act / Assert
        Assert.Throws<ArgumentNullException>(() => userService.InsertUser(null!));
    }

    [Fact]
    public async Task UserService_InsertUser_Test()
    {
        //Arrange
        User userToCreate = UserTestData.GetUser();
        IUserRepository userRepository = Substitute.For<IUserRepository>();
        userRepository.InsertUser(userToCreate).Returns(userToCreate);
        UserService userService = new(userRepository);

        //Act
        User createdUser = await userService.InsertUser(userToCreate);

        //Assert
        Assert.Equal(userToCreate, createdUser);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-5)]
    public void UserService_DeleteUser_InvalidUserId_Test(int invalidUserId)
    {
        //Arrange
        IUserRepository userRepository = Substitute.For<IUserRepository>();
        UserService userService = new(userRepository);

        //Act / Assert
        Assert.Throws<ArgumentException>(() => userService.DeleteUser(invalidUserId));
    }

    [Fact]
    public async Task UserService_DeleteUser_ValidUserId_Test()
    {
        //Arrange
        int userId = 11;
        long deletedUserCount = 1;
        IUserRepository userRepository = Substitute.For<IUserRepository>();
        userRepository.DeleteUser(userId).Returns(deletedUserCount);
        UserService userService = new(userRepository);

        //Act
        var numberOfDeletedUser = await userService.DeleteUser(userId);

        //Assert
        Assert.Equal(deletedUserCount, numberOfDeletedUser);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-4)]
    [InlineData(-24)]
    public void UserService_UpdateUser_InvalidUserId_Test(int invalidUserId)
    {
        //Arrange
        IUserRepository userRepository = Substitute.For<IUserRepository>();
        UserService userService = new(userRepository);
        User updateUser = UserTestData.GetUser();

        //Act / Assert
        Assert.ThrowsAsync<ArgumentException>(async () => await userService.UpdateUser(invalidUserId, updateUser));
    }

    [Fact]
    public void UserService_UpdateUser_NullUser_Test()
    {
        //Arrange
        int userId = 8;
        IUserRepository userRepository = Substitute.For<IUserRepository>();
        UserService userService = new(userRepository);
        User updateUser = UserTestData.GetUser();

        //Act / Assert
        Assert.ThrowsAsync<ArgumentNullException>(async () => await userService.UpdateUser(userId, null!));
    }

    [Fact]
    public async Task UserService_UpdateUser_Test()
    {
        //Arrange
        int userId = 8;
        User updateUser = UserTestData.GetUser();
        IUserRepository userRepository = Substitute.For<IUserRepository>();
        userRepository.UpdateUser(userId, updateUser).Returns(updateUser);
        UserService userService = new(userRepository);

        //Act
        var updatedUser = await userService.UpdateUser(userId, updateUser);

        //Assert
        Assert.Equal(updateUser, updatedUser);
        await userRepository.DidNotReceiveWithAnyArgs().InsertUser(default);
        await userRepository.ReceivedWithAnyArgs(1).UpdateUser(default,default!);
    }

    [Fact]
    public async Task UserService_UpdateUser_UserNotExists_Test()
    {
        //Arrange
        int userId = 8;
        User updateUser = UserTestData.GetUser();
        IUserRepository userRepository = Substitute.For<IUserRepository>();
        userRepository.UpdateUser(userId, updateUser).Returns((User)null!);
        userRepository.InsertUser(updateUser).Returns(updateUser);
        UserService userService = new(userRepository);

        //Act
        var updatedUser = await userService.UpdateUser(userId, updateUser);

        //Assert
        Assert.Equal(updateUser, updatedUser);
        await userRepository.ReceivedWithAnyArgs(1).InsertUser(default);
        await userRepository.ReceivedWithAnyArgs(1).UpdateUser(default, default!);
    }
}

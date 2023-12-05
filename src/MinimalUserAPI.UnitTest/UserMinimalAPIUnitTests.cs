using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http.HttpResults;
using MinimalUserAPI.Application;
using MinimalUserAPI.Application.Entity;
using MinimalUserAPI.Application.Interfaces;
using MinimalUserAPI.UnitTest.TestData;
using NSubstitute;

namespace MinimalUserAPI.UnitTest;

public class MinimalAPIUnitTests
{
    [Fact]
    public async void MinimalUserAPI_GetUsersTest()
    {
        //Arrange
        IUserService userService = Substitute.For<IUserService>();
        userService.GetUsers().Returns(UserTestData.GetUsers());

        //Act
        Ok<IEnumerable<User>> response = await UserAPIV01Endpoints.GetUsers(userService);

        //Assert
        Assert.IsType<Ok<IEnumerable<User>>>(response);

        Assert.NotNull(response.Value);

        Assert.Equal(UserTestData.GetUsers(), response.Value);
    }

    [Fact]
    public async void MinimalUserAPI_AddNewUser_InvalidUser_Test()
    {
        //Arrange
        IUserService userService = Substitute.For<IUserService>();
        userService.InsertUser(default!).ReturnsForAnyArgs(new User());
        IValidator<User> userValidator = Substitute.For<IValidator<User>>();
        userValidator.Validate(default!).ReturnsForAnyArgs(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("userId", "user Id is negative") }));

        //Act
        var response = await UserAPIV01Endpoints.CreateUser(new User(), userValidator, userService);

        //Assert
        Assert.IsType<Results<Created<User>, ValidationProblem>>(response);

        Assert.IsType<ValidationProblem>(response.Result);
    }

    [Fact]
    public async void MinimalUserAPI_AddNewUser_ValidUser_Test()
    {
        //Arrange
        var newUser = UserTestData.GetUser();
        IUserService userService = Substitute.For<IUserService>();
        userService.InsertUser(default!).ReturnsForAnyArgs(newUser);
        IValidator<User> userValidator = Substitute.For<IValidator<User>>();
        userValidator.Validate(default!).ReturnsForAnyArgs(new ValidationResult());

        //Act
        var response = await UserAPIV01Endpoints.CreateUser(newUser, userValidator, userService);

        //Assert
        Assert.IsType<Results<Created<User>, ValidationProblem>>(response);
        Assert.IsType<Created<User>>(response.Result);
        var castedResult = (Created<User>)response.Result;
        Assert.Equal(newUser, castedResult.Value);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-5)]
    public async void MinimalUserAPI_DeleteUser_NotFoundInvalidUserId_Test(int invalidUserId)
    {
        //Arrange
        IUserService userService = Substitute.For<IUserService>();

        //Act
        var response = await UserAPIV01Endpoints.DeleteUser(invalidUserId, userService);

        //Assert
        Assert.IsType<Results<Ok<long>, NotFound<UserNotFound>>>(response);
        Assert.IsType<NotFound<UserNotFound>>(response.Result);
    }

    [Fact]
    public async void MinimalUserAPI_DeleteUser_UserNotFound_Test()
    {
        //Arrange
        int userId = 11;
        int numberOfUserDeleted = 0;
        IUserService userService = Substitute.For<IUserService>();
        userService.DeleteUser(userId).Returns(numberOfUserDeleted);

        //Act
        var response = await UserAPIV01Endpoints.DeleteUser(userId, userService);

        //Assert
        Assert.IsType<Results<Ok<long>, NotFound<UserNotFound>>>(response);
        Assert.IsType<NotFound<UserNotFound>>(response.Result);
    }

    [Fact]
    public async void MinimalUserAPI_DeleteUser_UserDeleted_Test()
    {
        //Arrange
        int userId = 11;
        int numberOfUserDeleted = 1;
        IUserService userService = Substitute.For<IUserService>();
        userService.DeleteUser(userId).Returns(numberOfUserDeleted);

        //Act
        var response = await UserAPIV01Endpoints.DeleteUser(userId, userService);

        //Assert
        Assert.IsType<Results<Ok<long>, NotFound<UserNotFound>>>(response);
        Assert.IsType<Ok<long>>(response.Result);
        var responseAsOkResultObject = (Ok<long>)response.Result;
        Assert.Equal(numberOfUserDeleted, responseAsOkResultObject.Value);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-5)]
    public async void MinimalUserAPI_UpdateUser_NotFoundInvalidUserId_Test(int invalidUserId)
    {
        //Arrange
        IUserService userService = Substitute.For<IUserService>();
        IValidator<User> userValidator = Substitute.For<IValidator<User>>();
        User user = UserTestData.GetUser();

        //Act
        var response = await UserAPIV01Endpoints.UpdateUser(invalidUserId, user, userValidator, userService);

        //Assert
        Assert.IsType<Results<Ok<User>, NotFound<UserNotFound>, ValidationProblem>>(response);
        Assert.IsType<NotFound<UserNotFound>>(response.Result);
    }

    [Fact]
    public async void MinimalUserAPI_UpdateUser_InvalidUser_Test()
    {
        //Arrange
        int userId = 11;
        IUserService userService = Substitute.For<IUserService>();
        IValidator<User> userValidator = Substitute.For<IValidator<User>>();
        userValidator.Validate(default!).ReturnsForAnyArgs(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("userId", "user Id is negative") }));
        User user = UserTestData.GetUser();

        //Act
        var response = await UserAPIV01Endpoints.UpdateUser(userId, user, userValidator, userService);

        //Assert
        Assert.IsType<Results<Ok<User>, NotFound<UserNotFound>, ValidationProblem>>(response);
        Assert.IsType<ValidationProblem>(response.Result);
    }

    [Fact]
    public async void MinimalUserAPI_UpdateUser_ValidUser_Test()
    {
        //Arrange
        int userId = 11;
        User user = UserTestData.GetUser();
        IUserService userService = Substitute.For<IUserService>();
        userService.UpdateUser(default!, default!).ReturnsForAnyArgs(user);
        IValidator<User> userValidator = Substitute.For<IValidator<User>>();
        userValidator.Validate(default!).ReturnsForAnyArgs(new ValidationResult());

        //Act
        var response = await UserAPIV01Endpoints.UpdateUser(userId, user, userValidator, userService);

        //Assert
        Assert.IsType<Results<Ok<User>, NotFound<UserNotFound>, ValidationProblem>>(response);
        Assert.IsType<Ok<User>>(response.Result);
        User responseUser = ((Ok<User>)response.Result).Value!;
        Assert.NotNull(responseUser);
        Assert.Equal(user, responseUser);
    }
}
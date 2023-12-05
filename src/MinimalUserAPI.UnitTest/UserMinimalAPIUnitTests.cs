using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http.HttpResults;
using MinimalUserAPI.Application;
using MinimalUserAPI.Application.Entity;
using MinimalUserAPI.Application.Interfaces;
using NSubstitute;

namespace MinimalUserAPI.UnitTest;

public class MinimalAPIUnitTests
{
    [Fact]
    public async void MinimalUserAPI_GetUsersTest()
    {
        //Arrange
        IUserService userRepository = Substitute.For<IUserService>();
        userRepository.GetUsers().Returns(GetUsers());

        //Act
        Ok<IEnumerable<User>> response = await UserAPIV01Endpoints.GetUsers(userRepository);

        //Assert
        Assert.IsType<Ok<IEnumerable<User>>>(response);

        Assert.NotNull(response.Value);

        Assert.Equal(GetUsers(), response.Value);
    }

    [Fact]
    public async void MinimalUserAPI_AddNewUser_InvalidUser_Test()
    {
        //Arrange
        IUserService userRepository = Substitute.For<IUserService>();
        userRepository.InsertUser(default!).ReturnsForAnyArgs(new User());
        IValidator<User> userValidator = Substitute.For<IValidator<User>>();
        userValidator.Validate(default!).ReturnsForAnyArgs(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("userId", "user Id is negative") }));

        //Act
        var response = await UserAPIV01Endpoints.CreateUser(new User(), userValidator, userRepository);

        //Assert
        Assert.IsType<Results<Created<User>, ValidationProblem>>(response);

        Assert.IsType<ValidationProblem>(response.Result);
    }

    [Fact]
    public async void MinimalUserAPI_AddNewUser_ValidUser_Test()
    {
        //Arrange
        var newUser = GetUser();
        IUserService userRepository = Substitute.For<IUserService>();
        userRepository.InsertUser(default!).ReturnsForAnyArgs(newUser);
        IValidator<User> userValidator = Substitute.For<IValidator<User>>();
        userValidator.Validate(default!).ReturnsForAnyArgs(new ValidationResult());

        //Act
        var response = await UserAPIV01Endpoints.CreateUser(newUser, userValidator, userRepository);

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
        IUserService userRepository = Substitute.For<IUserService>();

        //Act
        var response = await UserAPIV01Endpoints.DeleteUser(invalidUserId, userRepository);

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
        IUserService userRepository = Substitute.For<IUserService>();
        userRepository.DeleteUser(userId).Returns(numberOfUserDeleted);

        //Act
        var response = await UserAPIV01Endpoints.DeleteUser(userId, userRepository);

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
        IUserService userRepository = Substitute.For<IUserService>();
        userRepository.DeleteUser(userId).Returns(numberOfUserDeleted);

        //Act
        var response = await UserAPIV01Endpoints.DeleteUser(userId, userRepository);

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
        IUserService userRepository = Substitute.For<IUserService>();
        IValidator<User> userValidator = Substitute.For<IValidator<User>>();
        User user = GetUser();

        //Act
        var response = await UserAPIV01Endpoints.UpdateUser(invalidUserId, user, userValidator, userRepository);

        //Assert
        Assert.IsType<Results<Ok<User>, NotFound<UserNotFound>, ValidationProblem>>(response);
        Assert.IsType<NotFound<UserNotFound>>(response.Result);
    }

    [Fact]
    public async void MinimalUserAPI_UpdateUser_InvalidUser_Test()
    {
        //Arrange
        int userId = 11;
        IUserService userRepository = Substitute.For<IUserService>();
        IValidator<User> userValidator = Substitute.For<IValidator<User>>();
        userValidator.Validate(default!).ReturnsForAnyArgs(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("userId", "user Id is negative") }));
        User user = GetUser();

        //Act
        var response = await UserAPIV01Endpoints.UpdateUser(userId, user, userValidator, userRepository);

        //Assert
        Assert.IsType<Results<Ok<User>, NotFound<UserNotFound>, ValidationProblem>>(response);
        Assert.IsType<ValidationProblem>(response.Result);
    }

    [Fact]
    public async void MinimalUserAPI_UpdateUser_ValidUser_Test()
    {
        //Arrange
        int userId = 11;
        User user = GetUser();
        IUserService userRepository = Substitute.For<IUserService>();
        userRepository.UpdateUser(default!, default!).ReturnsForAnyArgs(user);
        IValidator<User> userValidator = Substitute.For<IValidator<User>>();
        userValidator.Validate(default!).ReturnsForAnyArgs(new ValidationResult());

        //Act
        var response = await UserAPIV01Endpoints.UpdateUser(userId, user, userValidator, userRepository);

        //Assert
        Assert.IsType<Results<Ok<User>, NotFound<UserNotFound>, ValidationProblem>>(response);
        Assert.IsType<Ok<User>>(response.Result);
        User responseUser = ((Ok<User>)response.Result).Value!;
        Assert.NotNull(responseUser);
        Assert.Equal(user, responseUser);
    }

    private User GetUser() => new User
    {
        Id = 3,
        Name = "Kurtis Test",
        UserName = "Elwyn Forrest",
        Email = "Telly.Hoeger@gmail.com",
        Address = new Address
        {
            Street = "Raptor Trail",
            Suite = "Suite 244",
            City = "Budapest",
            ZipCode = "1099",
            Geo = new Geo
            {
                Lat = 24.8918,
                Lng = 21.8984
            }
        },
        Phone = "210.067.4543",
        Website = "elvis.com",
        Company = new Company
        {
            Name = "Johns and Johns group",
            CatchPhrase = "Configurable multimedia task-force back",
            Bs = "something"
        }
    };

    private IEnumerable<User> GetUsers()
    {
        return new List<User>
        {
            new User
            {
                Id = 1,
                Name = "Kurtis Weissnat",
                UserName = "Elwyn.Skiles",
                Email = "Telly.Hoeger@billy.biz",
                Address = new Address
                {
                    Street =  "Rex Trail",
                    Suite = "Suite 280",
                    City = "Howemouth",
                    ZipCode = "58804-1099",
                    Geo = new Geo
                    {
                        Lat = 24.8918,
                        Lng = 21.8984
                    }
                },
                Phone = "210.067.6132",
                Website = "elvis.io",
                Company = new Company
                {
                    Name = "Johns Group",
                    CatchPhrase = "Configurable multimedia task-force",
                    Bs =  "generate enterprise e-tailers"
                }
            },
                        new User
            {
                Id = 2,
                Name = "Glenna Reichert",
                UserName = "Delphine",
                Email = "Chaim_McDermott@dana.io",
                Address = new Address
                {
                    Street =  "Dayna Park",
                    Suite = "Suite 449",
                    City = "Bartholomebury",
                    ZipCode = "76495-3109",
                    Geo = new Geo
                    {
                        Lat = 24.6463,
                        Lng = -168.8889
                    }
                },
                Phone = "(775)976-6794 x41206",
                Website = "conrad.com",
                Company = new Company
                {
                    Name = "Yost and Sons",
                    CatchPhrase = "Switchable contextually-based project",
                    Bs =  "aggregate real-time technologies"
                }
            }
        };
    }
}
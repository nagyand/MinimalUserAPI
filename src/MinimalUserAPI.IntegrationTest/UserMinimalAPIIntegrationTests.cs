using System.Net;
using Microsoft.Extensions.DependencyInjection;
using MinimalUserAPI.Infrastructure.DbContext;
using MongoDB.Driver;
using MinimalUserAPI.Application.Entity;
using Newtonsoft.Json;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using MinimalUserAPI.IntegrationTest.TestData;

namespace MinimalUserAPI.IntegrationTest;

public class UserMinimalAPIIntegrationTests: IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> factory;
    private readonly HttpClient httpClient;

    public UserMinimalAPIIntegrationTests(TestWebApplicationFactory<Program> factory)
    {
        this.factory = factory;
        httpClient = factory.CreateClient();
        InitUserDb();
    }


    [Theory]
    [InlineData(-1)]
    [InlineData(-5)]
    public async Task MinimalUserAPI_PostUserWithInvalidParameters_Test(int invalidUserId)
    {
        //Arrange
        var errorMessage = "'Id' must be greater than '0'.";
        var newUser = UserTestData.GetUser(invalidUserId);

        //Act
        var response = await httpClient.PostAsJsonAsync("/api/v1/users", newUser);

        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var problemResult = await response.Content.ReadFromJsonAsync<HttpValidationProblemDetails>();

        Assert.NotNull(problemResult?.Errors);

        Assert.NotNull(response);
        Assert.Collection(problemResult.Errors, (error) => Assert.Equal(errorMessage, error.Value.First()));
    }


    [Fact]
    public async Task MinimalUserAPI_PostUserWithValidParameters_Test()
    {
        //Arrange
        var users = UserTestData.GetInitUsers();
        var newUser = UserTestData.GetUser();

        users.Add(newUser);

        //Act
        var postResponse = await httpClient.PostAsJsonAsync("/api/v1/users", newUser);

        //Assert
        Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

        var response = await httpClient.GetFromJsonAsync<List<User>>("/api/v1/users");

        Assert.NotNull(response);
        Assert.Equal(users, response);
    }

    [Fact]
    public async Task MinimalUserAPI_PutUserWithValidParameters_Test()
    {
        //Arrange
        var user = UserTestData.GetInitUsers().FirstOrDefault();
        var userToUpdate = user with { UserName = "Test Erno" };

        //Act
        var response = await httpClient.PutAsJsonAsync($"/api/v1/users/{userToUpdate.Id}", userToUpdate);

        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        User updatedUser = JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());

        Assert.NotNull(response);
        Assert.Equal(userToUpdate, updatedUser);
    }

    [Fact]
    public async Task MinimalUserAPI_DeleteWithValidParameters_Test()
    {
        //Arrange
        var user = UserTestData.GetInitUsers().FirstOrDefault();

        //Act
        var response = await httpClient.DeleteAsync($"/api/v1/users/{user.Id}");

        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }



    private void InitUserDb()
    {
        var users = UserTestData.GetInitUsers();
        using (var scope = factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetService<UserDbContext>();
            if (db != null && (db.Users.Find(_ => true).ToList()).Count != 0)
            {
                db.Users.DeleteMany(_ => true);
            }
            db.Users.InsertMany(users);
        }
    }
}
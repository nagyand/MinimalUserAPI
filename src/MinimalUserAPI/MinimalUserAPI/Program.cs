using MinimalUserAPI.Application.Entity;
using MinimalUserAPI.Application.Interfaces;
using MinimalUserAPI.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureInfrastructure(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapDelete("/user/{userId:int}", async (int userId, IUserRepository repository) =>
{
    if (userId <= 0)
    {
        return Results.NotFound($"User with id {userId} not found");
    }
    var deleteCount = await repository.DeleteUser(userId);
    if (deleteCount == 0)
    {
        return Results.NotFound(new { error = $"User with id {userId} not found" });
    }
    return Results.Ok(deleteCount);
});

app.MapPut("/users/{userId:int}", async (int userId, User user, IUserRepository repository) =>
{
    if (userId <= 0)
    {
        return Results.NotFound(new { error = $"User with id {userId} not found" });
    }
    var updatedUser = await repository.UpdateUser(userId, user);
    return Results.Ok(updatedUser);
});

app.MapGet("/users", async (IUserRepository userRepository) =>
{
    var users = await userRepository.GetUsers();
    return Results.Ok(users);
});

app.MapPost("/users", async (User newUser, IUserRepository UserRepository) =>
{
    var result = await UserRepository.InsertUser(newUser);
    return Results.Created($"/user/{result.Id}", result);
});

app.Run();


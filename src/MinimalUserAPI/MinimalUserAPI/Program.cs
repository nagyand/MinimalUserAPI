using FluentValidation;
using MinimalUserAPI.Application.Entity;
using MinimalUserAPI.Application.Extensions;
using MinimalUserAPI.Application.Interfaces;
using MinimalUserAPI.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureInfrastructure(builder.Configuration);
builder.Services.ConfigureValidations();
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

app.MapDelete("/api/v1/users/{userId:int}", async (int userId, IUserRepository repository) =>
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
})
.WithTags("User API");

app.MapPut("/api/v1/users/{userId:int}", async (int userId, User user, IValidator<User> userValidator, IUserRepository repository) =>
{
    if (userId <= 0)
    {
        return Results.NotFound(new { error = $"User with id {userId} not found" });
    }
    var validationResult = userValidator.Validate(user);
    if (!validationResult.IsValid)
    {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }
    var updatedUser = await repository.UpdateUser(userId, user);
    return Results.Ok(updatedUser);
})
.WithTags("User API");

app.MapGet("/api/v1/users", async (IUserRepository userRepository) =>
{
    var users = await userRepository.GetUsers();
    return Results.Ok(users);
})
.WithTags("User API");

app.MapPost("/api/v1/users", async (User newUser, IValidator<User> userValidator, IUserRepository UserRepository) =>
{
    var validationResult = userValidator.Validate(newUser);
    if (!validationResult.IsValid)
    {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }
    var result = await UserRepository.InsertUser(newUser);
    return Results.Created($"/user/{result.Id}", result);
})
.WithTags("User API");

app.Run();


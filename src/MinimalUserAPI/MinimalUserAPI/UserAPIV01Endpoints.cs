using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using MinimalUserAPI.Application;
using MinimalUserAPI.Application.Entity;
using MinimalUserAPI.Application.Interfaces;

namespace MinimalUserAPI;

public static class UserAPIV01Endpoints
{
    public static RouteGroupBuilder MapUserApiV1(this RouteGroupBuilder group)
    {
        group.MapPost("/", CreateUser);
        group.MapGet("/", GetUsers);
        group.MapPut("/{userId:int}", UpdateUser);
        group.MapDelete("/{userId:int}", DeleteUser);
        return group;
    }

    public static async Task<Results<Created<User>, ValidationProblem>> CreateUser(User newUser, IValidator<User> userValidator, IUserRepository UserRepository)
    {
        var validationResult = userValidator.Validate(newUser);
        if (!validationResult.IsValid)
        {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }
        var result = await UserRepository.InsertUser(newUser);
        return TypedResults.Created($"/user/{result.Id}", result);
    }

    public static async Task<Ok<IEnumerable<User>>> GetUsers(IUserRepository userRepository)
    {
        var users = await userRepository.GetUsers();
        return TypedResults.Ok(users);
    }

    public static async Task<Results<Ok<User>, NotFound<UserNotFound>, ValidationProblem>> UpdateUser(int userId, User user, IValidator<User> userValidator, IUserRepository userRepository)
    {
        if (userId <= 0)
        {
            return TypedResults.NotFound(new UserNotFound($"User not found with id: '{userId}'"));
        }
        var validationResult = userValidator.Validate(user);
        if (!validationResult.IsValid)
        {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }
        var updatedUser = await userRepository.UpdateUser(userId, user);
        return TypedResults.Ok(updatedUser);
    }

    public static async Task<Results<Ok<long>, NotFound<UserNotFound>>> DeleteUser(int userId, IValidator<User> userValidator, IUserRepository userRepository)
    {
        if (userId <= 0)
        {
            return TypedResults.NotFound(new UserNotFound($"User not found with id: '{userId}'"));
        }
        var deleteCount = await userRepository.DeleteUser(userId);
        if (deleteCount == 0)
        {
            return TypedResults.NotFound(new UserNotFound($"User not found with id: '{userId}'"));
        }
        return TypedResults.Ok(deleteCount);
    }
}

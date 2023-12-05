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

    public static async Task<Results<Created<User>, ValidationProblem>> CreateUser(User newUser, IValidator<User> userValidator, IUserService userService)
    {
        var validationResult = userValidator.Validate(newUser);
        if (!validationResult.IsValid)
        {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }
        var result = await userService.InsertUser(newUser);
        return TypedResults.Created($"/user/{result.Id}", result);
    }

    public static async Task<Ok<IEnumerable<User>>> GetUsers(IUserService userService)
    {
        var users = await userService.GetUsers();
        return TypedResults.Ok(users);
    }

    public static async Task<Results<Ok<User>, NotFound<UserNotFound>, ValidationProblem>> UpdateUser(int userId, User user, IValidator<User> userValidator, IUserService userService)
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
        var updatedUser = await userService.UpdateUser(userId, user);
        return TypedResults.Ok(updatedUser);
    }

    public static async Task<Results<Ok<long>, NotFound<UserNotFound>>> DeleteUser(int userId, IUserService userService)
    {
        if (userId <= 0)
        {
            return TypedResults.NotFound(new UserNotFound($"User not found with id: '{userId}'"));
        }
        var deleteCount = await userService.DeleteUser(userId);
        if (deleteCount == 0)
        {
            return TypedResults.NotFound(new UserNotFound($"User not found with id: '{userId}'"));
        }
        return TypedResults.Ok(deleteCount);
    }
}

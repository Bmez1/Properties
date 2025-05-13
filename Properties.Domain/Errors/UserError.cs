using Crosscutting;

namespace Properties.Domain.Errors;

public static class UserError
{
    public static Error NotFound(Guid userId) => Error.NotFound(
        "Users.NotFound",
        $"The user with the Id = '{userId}' was not found");

    public static Error Unauthorized => Error.Failure(
        "Users.Unauthorized",
        "You are only authorized to access your own information.");

    public static readonly Error NotFoundByEmail = Error.NotFound(
    "Users.NotFoundByEmail",
    "The user with the specified email was not found");

    public static readonly Error EmailNotUnique = Error.Conflict(
    "Users.EmailNotUnique",
    "The provided email is not unique");
}


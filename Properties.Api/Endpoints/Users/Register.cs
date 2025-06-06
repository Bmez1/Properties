﻿using Crosscutting;

using MediatR;

using Properties.Api.HttpResponse;
using Properties.Application.UseCases.Users.Register;

namespace Properties.Api.Endpoints.Users;

internal sealed class Register : IEndpoint
{
    public sealed record RegisterUserRequest(string Email, string Password);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users/register", async (RegisterUserRequest request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new RegisterUserCommand(
                request.Email,
                request.Password);

            Result<Guid> result = await sender.Send(command, cancellationToken);

            return result.ToHttpResponse();
        })
        .WithTags(Tags.Users);
    }
}

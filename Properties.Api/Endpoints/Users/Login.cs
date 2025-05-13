using Crosscutting;

using MediatR;

using Properties.Api.HttpResponse;
using Properties.Application.UseCases.Users;
using Properties.Application.UseCases.Users.Login;

namespace Properties.Api.Endpoints.Users;

internal sealed class Login : IEndpoint
{
    public sealed record Request(string Email, string Password);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users/login", async (Request request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new LoginUserCommand(request.Email, request.Password);

            Result<string> result = await sender.Send(command, cancellationToken);

            return result.ToHttpResponse();
        })
        .WithTags(Tags.Users);
    }
}

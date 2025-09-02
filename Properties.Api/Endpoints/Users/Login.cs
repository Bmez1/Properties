using Crosscutting;

using MediatR;

using Properties.Api.HttpResponse;
using Properties.Application.UseCases.Users.Login;

namespace Properties.Api.Endpoints.Users;

public sealed class Login : IEndpoint
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
        .WithSummary("Iniciar sesión.")
        .WithDescription("Use su correo y su contraseña para iniciar sesión.")
        .WithTags(Tags.Users);
    }
}

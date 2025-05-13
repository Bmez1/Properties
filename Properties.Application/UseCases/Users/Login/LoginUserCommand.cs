using Crosscutting;

using MediatR;

namespace Properties.Application.UseCases.Users.Login
{
    public sealed record LoginUserCommand(string Email, string Password) : IRequest<Result<string>>;
}

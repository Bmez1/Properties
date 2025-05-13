using Crosscutting;

using MediatR;

namespace Properties.Application.UseCases.Users.Register
{
    public sealed record RegisterUserCommand(string Email, string Password) : IRequest<Result<Guid>>;
}

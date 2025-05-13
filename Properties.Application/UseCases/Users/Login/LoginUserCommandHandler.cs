using Crosscutting;

using MediatR;

using Properties.Application.Interfaces;
using Properties.Domain.Entities;
using Properties.Domain.Errors;

namespace Properties.Application.UseCases.Users.Login
{
    public class LoginUserCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    ITokenProvider tokenProvider) : IRequestHandler<LoginUserCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            User? user = await userRepository.GetByEmailAsync(request.Email, cancellationToken);

            if (user is null)
            {
                return Result.Failure<string>(UserError.NotFoundByEmail);
            }

            bool verified = passwordHasher.Verify(request.Password, user.PasswordHash);

            if (!verified)
            {
                return Result.Failure<string>(UserError.NotFoundByEmail);
            }

            string token = tokenProvider.Create(user);

            return token;
        }
    }
}

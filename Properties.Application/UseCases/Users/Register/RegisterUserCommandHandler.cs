using Crosscutting;

using MediatR;

using Properties.Application.Interfaces;
using Properties.Domain.Entities;
using Properties.Domain.Errors;

namespace Properties.Application.UseCases.Users.Register
{
    public class RegisterUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork
        ) : IRequestHandler<RegisterUserCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            if (await userRepository.ExistsByEmailAsync(request.Email, cancellationToken))
            {
                return Result.Failure<Guid>(UserError.EmailNotUnique);
            }

            var user = User.Create(request.Email, passwordHasher.Hash(request.Password));

            await userRepository.AddAsync(user);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return user.Id;
        }
    }
}

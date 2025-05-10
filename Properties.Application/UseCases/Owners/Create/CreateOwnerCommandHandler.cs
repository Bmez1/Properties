using Crosscutting;

using MediatR;

using Properties.Application.Interfaces;
using Properties.Domain.Entities;

namespace Properties.Application.UseCases.Owners.Create
{
    public class CreateOwnerCommandHandler(IOwnerRepository ownerRepository) : IRequestHandler<CreateOwnerCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(CreateOwnerCommand request, CancellationToken cancellationToken)
        {
            var newOwner = Owner.Create(request.Name, request.Address, request.Birthday, request.Photo);
            await ownerRepository.CreateAsync(newOwner);

            return newOwner.Id;
        }
    }
}

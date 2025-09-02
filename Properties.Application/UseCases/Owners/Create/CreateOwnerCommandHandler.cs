using Crosscutting;

using MediatR;

using Properties.Application.Interfaces;
using Properties.Domain.Entities;

namespace Properties.Application.UseCases.Owners.Create
{
    public class CreateOwnerCommandHandler(
        IOwnerRepository ownerRepository, 
        IUnitOfWork unitOfWork,
        IBlobStorageService blobStorageService) : IRequestHandler<CreateOwnerCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(CreateOwnerCommand request, CancellationToken cancellationToken)
        {
            string photoPath = string.Empty;
            if (request.Photo is not null)
            {
                var fileName = Path.Combine(Owner.Directory, $"{DateTime.UtcNow:yyyyMMddHHmmss}{request.Photo.FileName}");

                photoPath = await blobStorageService.UploadFileAsync(request.Photo.OpenReadStream, fileName, cancellationToken: cancellationToken);
            }

            var newOwner = Owner.Create(request.Name, request.Address, request.Birthday, photoPath);
            await ownerRepository.CreateAsync(newOwner);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return newOwner.Id;
        }
    }
}

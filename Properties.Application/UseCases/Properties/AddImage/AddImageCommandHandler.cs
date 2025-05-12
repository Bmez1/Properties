using Crosscutting;

using MediatR;

using Properties.Application.Interfaces;
using Properties.Application.UseCases.Properties.Dtos;
using Properties.Domain.Errors;

using Property = Properties.Domain.Entities.Property;

namespace Properties.Application.UseCases.Properties.AddImage
{
    public class AddImageCommandHandler(
        IPropertyRepository propertyRepository,
        IUnitOfWork unitOfWork,
        IBlobStorageService blobStorageService) : IRequestHandler<AddImageCommand, Result<AddImageResponseDto>>
    {
        public async Task<Result<AddImageResponseDto>> Handle(AddImageCommand request, CancellationToken cancellationToken)
        {
            var property = await propertyRepository.GetByIdAsync(request.PropertyId);

            if (property is null)
                return Result.Failure<AddImageResponseDto>(PropertyError.NotFoundById);

            var fileName = Path.Combine(Property.NameDirectory, $"{DateTime.UtcNow:yyyyMMddHHmmss}{request.FileUpload.FileName}");

            var url = await blobStorageService.UploadFileAsync(request.FileUpload.OpenReadStream, fileName, cancellationToken: cancellationToken);

            property.AddImage(url);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new AddImageResponseDto
            {
                Image = url,
                PropertyId = request.PropertyId
            };
        }
    }
}

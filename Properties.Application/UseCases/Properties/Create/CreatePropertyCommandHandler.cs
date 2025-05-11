using Crosscutting;

using MediatR;

using Properties.Application.Interfaces;
using Properties.Application.UseCases.Properties.Dtos;
using Properties.Application.UseCases.Properties.Mappers;
using Properties.Domain.Entities;
using Properties.Domain.Errors;

namespace Properties.Application.UseCases.Properties.Create
{
    public class CreatePropertyCommandHandler(
        IPropertyRepository propertyRepository,
        IOwnerRepository ownerRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<CreatePropertyCommand, Result<CreatePropertyResponseDto>>
    {
        public async Task<Result<CreatePropertyResponseDto>> Handle(CreatePropertyCommand request, CancellationToken cancellationToken)
        {
            var newProperty = Property.Create(request.Name, request.Address, request.Price, request.Year, request.OwnerId);

            if (request.OwnerId is not null)
            {
                if (!await ownerRepository.ExistsByIdAsync(request.OwnerId.Value))
                    return Result.Failure<CreatePropertyResponseDto>(OwnerError.NotFoundById);

                newProperty.AddTrace(request.Trace.Name, request.Trace.DateSale, request.Trace.Value, request.Trace.Tax);
            }

            await propertyRepository.CreateAsync(newProperty);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return newProperty.MapToCreatePropertyResponseDto();
        }
    }
}

using Crosscutting;

using MediatR;

using Properties.Application.UseCases.Properties.Dtos;
using Properties.Application.UseCases.Properties.Mappers;
using Properties.Domain.Entities;
using Properties.Domain.Interfaces;

namespace Properties.Application.UseCases.Properties.Create
{
    public class CreatePropertyCommandHandler(
        IPropertyRepository propertyRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<CreatePropertyCommand, Result<CreatePropertyResponseDto>>
    {
        public async Task<Result<CreatePropertyResponseDto>> Handle(CreatePropertyCommand request, CancellationToken cancellationToken)
        {
            var newProperty = Property.Create(request.Name, request.Address, request.Price, request.Year, request.OwnerId);

            await propertyRepository.CreateAsync(newProperty);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return newProperty.MapToCreatePropertyResponseDto();
        }
    }
}

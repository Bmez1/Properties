using Crosscutting;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Properties.Application.Interfaces;
using Properties.Application.UseCases.Properties.Dtos;

namespace Properties.Application.UseCases.Properties.List
{
    public class ListQueryHandler(IPropertyRepository propertyRepository) : IRequestHandler<ListQuery, Result<IEnumerable<PropertyResponseDto>>>
    {
        public async Task<Result<IEnumerable<PropertyResponseDto>>> Handle(ListQuery request, CancellationToken cancellationToken)
        {
            var properties = propertyRepository.GetAll(asNoTracking: true);

            var propertiesDto = await properties.Select(p => new PropertyResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Address = p.Address,
                CodeInternal = p.CodeInternal,
                IsAvailable = p.OwnerId == null,
                Price = p.Price,
                Year = p.Year,
                OwnerId = p.OwnerId,
                OwnerName = p.OwnerId == null ? string.Empty : p.Owner.Name
            }).ToListAsync(cancellationToken);

            return Result.Success(propertiesDto.AsEnumerable(), totalData: propertiesDto.Count);
        }
    }
}

using Crosscutting;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Properties.Application.Interfaces;
using Properties.Application.UseCases.Properties.Dtos;

namespace Properties.Application.UseCases.Properties.Get
{
    public sealed record GetByIdQuery(Guid PropertyId) : IRequest<Result<PropertyResponseDto>>;

    public class GetByIdQueryHandler(IPropertyRepository propertyRepository) : IRequestHandler<GetByIdQuery, Result<PropertyResponseDto>>
    {
        public async Task<Result<PropertyResponseDto>> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            var properties = propertyRepository.GetAll(asNoTracking: true);

            var property = await properties
                .Where(p => request.PropertyId == p.Id)  
                .Select(p => new PropertyResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Address = p.Address,
                    CodeInternal = p.CodeInternal,
                    Price = p.Price,
                    Year = p.Year,
                    OwnerId = p.OwnerId,
                    OwnerName = p.Owner.Name,
                    ImagesCount = p.Images.Count,
                    ImagesUrl = p.Images.Where(i => i.Enabled).Select(i => i.File)
                }).FirstOrDefaultAsync(cancellationToken);

            return property;
        }
    }
}

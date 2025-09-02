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

            var pageNumber = request.Filter.PageNumber ?? 1;
            var pageSize = request.Filter.PageSize ?? 10;
            var filter = request.Filter;

            var propertiesDto = await properties
                .Where(p => request.Filter.OwnerName == null || p.Owner.Name.Contains(request.Filter.OwnerName))
                .Where(p => request.Filter.Address == null || p.Address.Contains(request.Filter.Address))
                .Where(p => !filter.MinPrice.HasValue || p.Price >= filter.MinPrice)
                .Where(p => !filter.MaxPrice.HasValue || p.Price <= filter.MaxPrice)
                .Where(p => !filter.MinYear.HasValue || p.Year >= filter.MinYear)
                .Where(p => !filter.MaxYear.HasValue || p.Year <= filter.MaxYear)
                .OrderBy(p => p.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
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
                }).ToListAsync(cancellationToken);

            return Result.Success(propertiesDto.AsEnumerable(), totalData: await properties.CountAsync(cancellationToken));
        }
    }
}

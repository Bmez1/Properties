using Crosscutting;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Properties.Application.Interfaces;
using Properties.Application.UseCases.PropertyTraces.Dtos;

namespace Properties.Application.UseCases.PropertyTraces.List
{
    public class ListTracesQueryHandler(IPropertyTraceRepository propertyTraceRepository) : IRequestHandler<ListTracesQuery, Result<IEnumerable<PropertyTraceResponseDto>>>
    {
        public async Task<Result<IEnumerable<PropertyTraceResponseDto>>> Handle(ListTracesQuery request, CancellationToken cancellationToken)
        {
            int page = request.PageNumber == 0 ? 1 : request.PageNumber;
            int pageSize = request.PageSize == 0 ? 10 : request.PageSize;

            var traces = propertyTraceRepository.GetAll()
                .Where(x => x.PropertyId == request.PropertyId);

            var result = await traces
                .OrderByDescending(x => x.DateSale)
                .Skip((page - 1) * request.PageSize)
                .Take(pageSize)
                .Select(x => new PropertyTraceResponseDto
                {
                    Id = x.Id,
                    PropertyId = x.PropertyId,
                    DateSale = x.DateSale,
                    Name = x.Name,
                    Tax = x.Tax,
                    Value = x.Value
                })
                .ToListAsync(cancellationToken);

            var totalData = await traces.CountAsync(cancellationToken);

            return Result.Success(result.AsEnumerable(), totalData: totalData);
        }
    }
}

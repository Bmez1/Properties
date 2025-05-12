using Crosscutting;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Properties.Application.Interfaces;
using Properties.Application.UseCases.Owners.Dtos;

namespace Properties.Application.UseCases.Owners.List
{
    public class ListQueryHandler(IOwnerRepository ownerRepository) : IRequestHandler<ListQuery, Result<IEnumerable<OwnerResponseDto>>>
    {
        public async Task<Result<IEnumerable<OwnerResponseDto>>> Handle(ListQuery request, CancellationToken cancellationToken)
        {
            var owners = await ownerRepository
                .GetAll()
                .Select(x => new OwnerResponseDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Address = x.Address,
                    Birthday = x.Birthday,
                    NumberProperties = x.Properties.Count,
                    Photo = x.Photo
                })
                .ToListAsync(cancellationToken);

            return Result.Success<IEnumerable<OwnerResponseDto>>(owners, owners.Count);
        }
    }
}

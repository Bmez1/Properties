using Crosscutting;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Properties.Application.Interfaces;
using Properties.Application.UseCases.Owners.Dtos;

namespace Properties.Application.UseCases.Owners.Get
{
    public class GetByIdQueryHandler(IOwnerRepository ownerRepository) : IRequestHandler<GetByIdQuery, Result<OwnerResponseDto>>
    {
        public async Task<Result<OwnerResponseDto>> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            var owner = await ownerRepository
                .GetAll()
                .Where(x => x.Id == request.OwnerId)
                .Select(x => new OwnerResponseDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Address = x.Address,
                    Birthday = x.Birthday,
                    NumberProperties = x.Properties.Count,
                    Photo = x.Photo
                }).FirstOrDefaultAsync(cancellationToken);

            return owner;
        }
    }
}

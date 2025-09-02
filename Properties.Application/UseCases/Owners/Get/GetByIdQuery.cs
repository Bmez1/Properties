using Crosscutting;

using MediatR;

using Properties.Application.UseCases.Owners.Dtos;

namespace Properties.Application.UseCases.Owners.Get
{
    public sealed record GetByIdQuery(Guid OwnerId) : IRequest<Result<OwnerResponseDto>>;
}

using Crosscutting;

using MediatR;

using Properties.Application.UseCases.Owners.Dtos;

namespace Properties.Application.UseCases.Owners.List
{
    public sealed record ListQuery() : IRequest<Result<IEnumerable<OwnerResponseDto>>>;
}

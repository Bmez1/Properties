using Crosscutting;

using MediatR;

using Properties.Application.UseCases.PropertyTraces.Dtos;

namespace Properties.Application.UseCases.PropertyTraces.List
{
    public sealed record ListTracesQuery(
        Guid PropertyId,
        int PageNumber,
        int PageSize) : IRequest<Result<IEnumerable<PropertyTraceResponseDto>>>;
}

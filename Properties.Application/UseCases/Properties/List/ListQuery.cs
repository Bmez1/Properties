using Crosscutting;

using MediatR;

using Properties.Application.UseCases.Properties.Dtos;

namespace Properties.Application.UseCases.Properties.List
{
    public record class ListQuery(PropertyFilterDto Filter) : IRequest<Result<IEnumerable<PropertyResponseDto>>>;
}

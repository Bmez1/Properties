using Crosscutting;

using MediatR;

using Properties.Application.UseCases.Properties.Dtos;

namespace Properties.Application.UseCases.Properties.List
{
    public record class ListQuery : IRequest<Result<IEnumerable<PropertyResponseDto>>>;
}

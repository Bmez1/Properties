using Crosscutting;

using MediatR;

using Properties.Application.UseCases.Properties.Dtos;

namespace Properties.Application.UseCases.Properties.Create
{
    public sealed record CreatePropertyCommand(
        string Name,
        string Address,
        decimal Price,
        int Year,
        Guid? OwnerId
    ) : IRequest<Result<CreatePropertyResponseDto>>;
}

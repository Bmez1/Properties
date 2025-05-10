using Crosscutting;

using MediatR;

namespace Properties.Application.UseCases.Owners.Create
{
    public sealed record CreateOwnerCommand(
        string Name,
        string Address,
        DateOnly Birthday,
        string? Photo
    ) : IRequest<Result<Guid>>;

}

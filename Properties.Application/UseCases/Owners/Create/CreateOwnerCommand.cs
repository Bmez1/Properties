using Crosscutting;

using MediatR;

using Properties.Application.UseCases.Properties.Create;

namespace Properties.Application.UseCases.Owners.Create
{
    public sealed record CreateOwnerCommand(
        string Name,
        string Address,
        DateOnly Birthday,
        string? Photo
    ) : IRequest<Result<Guid>>;

}

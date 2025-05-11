using Crosscutting;

using MediatR;

using Properties.Application.Interfaces;

namespace Properties.Application.UseCases.Owners.Create
{
    public sealed record CreateOwnerCommand(
        string Name,
        string Address,
        DateOnly Birthday,
        IFileUpload? Photo
    ) : IRequest<Result<Guid>>;

}

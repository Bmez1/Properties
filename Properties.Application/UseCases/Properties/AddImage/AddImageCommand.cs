using Crosscutting;

using MediatR;

using Properties.Application.Interfaces;

namespace Properties.Application.UseCases.Properties.AddImage
{
    public sealed record AddImageCommand(
        IFileUpload FileUpload,
        Guid PropertyId
        ): IRequest<Result<Guid>>;
}

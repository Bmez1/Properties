using Crosscutting;

using MediatR;

using Properties.Application.Interfaces;
using Properties.Application.UseCases.Properties.Dtos;

namespace Properties.Application.UseCases.Properties.AddImage
{
    public sealed record AddImageCommand(
        IFileUpload FileUpload,
        Guid PropertyId
        ): IRequest<Result<AddImageResponseDto>>;
}

using Crosscutting;

using MediatR;

using Properties.Application.UseCases.Properties.Dtos;

namespace Properties.Application.UseCases.Owners.AddProperty
{
    public sealed record AddPropertyCommand(
        Guid OwnerId,
        Guid PropertyId,
        PropertyTraceCreateDto Trace
    ) : IRequest<Result<Guid>>;
}

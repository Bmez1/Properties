using Crosscutting;

using MediatR;

using Properties.Application.UseCases.Properties.Dtos;

namespace Properties.Application.UseCases.Properties.Update
{
    public sealed record UpdatePropertyCommad(
        Guid PropertyId,
        string Name,
        string Address,
        decimal Price, 
        int Year, 
        Guid? OwnerId,
        PropertyTraceCreateDto Trace) : IRequest<Result<Guid>>;
}

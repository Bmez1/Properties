using Crosscutting;

using MediatR;

namespace Properties.Application.UseCases.Properties.ChangePrice
{
    public sealed record ChangePriceCommand(Guid PropertyId, decimal Price) : IRequest<Result<Guid>>;
}

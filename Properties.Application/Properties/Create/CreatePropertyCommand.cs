using MediatR;

namespace Properties.Application.Properties.Create
{
    public sealed record CreatePropertyCommand(
        string Name,
        string Address,
        decimal Price,
        string CodeInternal,
        int Year,
        Guid? OwnerId,
        DateTime CreatedAt
    ) : IRequest<Guid>;

}

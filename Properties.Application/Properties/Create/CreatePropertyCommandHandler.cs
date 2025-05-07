using MediatR;

namespace Properties.Application.Properties.Create
{
    public class CreatePropertyCommandHandler : IRequestHandler<CreatePropertyCommand, Guid>
    {
        public async Task<Guid> Handle(CreatePropertyCommand request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(Guid.NewGuid());
        }
    }
}

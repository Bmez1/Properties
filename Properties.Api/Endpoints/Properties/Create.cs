using MediatR;

using Properties.Application.Properties.Create;

namespace Properties.Api.Endpoints.Properties;

internal sealed class Create : IEndpoint
{
    public sealed class Request
    {
        public string Name { get; set; } = default!;
        public string Address { get; set; } = default!;
        public decimal Price { get; set; }
        public string CodeInternal { get; set; } = default!;
        public int Year { get; set; }
        public Guid? OwnerId { get; set; }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("properties", async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var command = new CreatePropertyCommand(
                request.Name,
                request.Address,
                request.Price,
                request.CodeInternal,
                request.Year,
                request.OwnerId,
                DateTime.UtcNow
            );

            var result = await mediator.Send(command, cancellationToken);

            return result;
        })
        .WithTags("Properties");
    }
}


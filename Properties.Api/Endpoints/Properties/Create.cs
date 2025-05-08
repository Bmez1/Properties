using MediatR;

using Properties.Api.HttpResponse;
using Properties.Application.UseCases.Properties.Create;

namespace Properties.Api.Endpoints.Properties;

internal sealed class Create : IEndpoint
{
    public sealed class CreatePropertyRequest
    {
        public string Name { get; set; } = default!;
        public string Address { get; set; } = default!;
        public decimal Price { get; set; }
        public int Year { get; set; }
        public Guid? OwnerId { get; set; }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("properties", async (CreatePropertyRequest request, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var command = new CreatePropertyCommand(
                request.Name,
                request.Address,
                request.Price,
                request.Year,
                request.OwnerId
            );

            var result = await mediator.Send(command, cancellationToken);

            return result.ToHttpResponse(); ;
        })
        .WithTags("Properties");
    }
}


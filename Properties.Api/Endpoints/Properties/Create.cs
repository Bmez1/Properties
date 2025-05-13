using MediatR;

using Properties.Api.HttpResponse;
using Properties.Application.UseCases.Properties.Create;
using Properties.Application.UseCases.Properties.Dtos;

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
        public PropertyTraceRequest? Trace { get; set; }
    }

    public class PropertyTraceRequest
    {
        public decimal Value { get; init; }
        public decimal Tax { get; init; }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("properties", async (CreatePropertyRequest request, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var trace = request.Trace is null ? null : 
                new PropertyTraceCreateDto
                { 
                    Value = request.Trace.Value,
                    Tax = request.Trace.Tax
                };

            var command = new CreatePropertyCommand(
                request.Name,
                request.Address,
                request.Price,
                request.Year,
                request.OwnerId,
                trace!
            );

            var result = await mediator.Send(command, cancellationToken);

            return result.ToHttpResponse(); ;
        })
        .RequireAuthorization()
        .WithTags(Tags.Properties);
    }
}


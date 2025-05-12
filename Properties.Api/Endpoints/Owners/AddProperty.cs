using MediatR;

using Properties.Api.HttpResponse;
using Properties.Application.UseCases.Owners.AddProperty;
using Properties.Application.UseCases.Properties.Dtos;

namespace Properties.Api.Endpoints.Owners;

internal sealed class AddProperty : IEndpoint
{
    public sealed class AddPropertyRequest
    {
        public Guid PropertyId { get; set; } = default!;
        public Guid OwnerId { get; set; } = default!;
        public AddPropertyTraceRequest Trace { get; set; } = default!;
    }

    public class AddPropertyTraceRequest
    {
        public decimal Value { get; init; }
        public decimal Tax { get; init; }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("owners/properties", async (AddPropertyRequest request, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var command = new AddPropertyCommand(
                request.OwnerId,
                request.PropertyId,
                new PropertyTraceCreateDto
                {
                    Value = request.Trace.Value,
                    Tax = request.Trace.Tax
                }
            );

            var result = await mediator.Send(command, cancellationToken);

            return result.ToHttpResponse();
        })
        .WithTags(Tags.Owners);
    }
}


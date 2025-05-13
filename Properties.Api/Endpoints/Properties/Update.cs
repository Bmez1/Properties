
using MediatR;

using Properties.Api.HttpResponse;
using Properties.Application.UseCases.Properties.Dtos;
using Properties.Application.UseCases.Properties.Update;

namespace Properties.Api.Endpoints.Properties
{
    public class Update : IEndpoint
    {
        public class UpdatePropertyRequest
        {
            public Guid PropertyId { get; init; }
            public string Name { get; init; } = default!;
            public string Address { get; init; } = default!;
            public decimal Price { get; init; }
            public int Year { get; init; }
            public Guid OwnerId { get; init; }
            public PropertyTraceUpdateRequest? Trace { get; init; }


            public static explicit operator UpdatePropertyCommad(UpdatePropertyRequest dto)
            {
                var trace = dto.Trace is null ? null : new PropertyTraceCreateDto
                {
                    Value = dto.Trace.Value,
                    Tax = dto.Trace.Tax
                };

                return new UpdatePropertyCommad
                (
                    dto.PropertyId,
                    dto.Name,
                    dto.Address,
                    dto.Price,
                    dto.Year,
                    dto.OwnerId,
                    trace!
                );
            }
        }

        public class PropertyTraceUpdateRequest
        {
            public decimal Value { get; init; }
            public decimal Tax { get; init; }
        }

        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("properties", async (UpdatePropertyRequest request, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send((UpdatePropertyCommad)request, cancellationToken);
                return result.ToHttpResponse(); ;
            })
            .RequireAuthorization()
            .WithTags(Tags.Properties)
            .WithDescription("Updates a property");
        }
    }
}

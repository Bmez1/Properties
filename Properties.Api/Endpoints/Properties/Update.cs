
using MediatR;

using Microsoft.AspNetCore.Mvc;

using Properties.Api.HttpResponse;
using Properties.Application.UseCases.Properties.Dtos;
using Properties.Application.UseCases.Properties.Update;

namespace Properties.Api.Endpoints.Properties
{
    public class Update : IEndpoint
    {
        public class UpdatePropertyRequest
        {
            public string Name { get; init; } = default!;
            public string Address { get; init; } = default!;
            public decimal Price { get; init; }
            public int Year { get; init; }
            public Guid OwnerId { get; init; }
            public PropertyTraceUpdateRequest? Trace { get; init; }


            public static UpdatePropertyCommad ToCommand(UpdatePropertyRequest dto, Guid propertyId)
            {
                var trace = dto.Trace is null ? null : new PropertyTraceCreateDto
                {
                    Value = dto.Trace.Value,
                    Tax = dto.Trace.Tax
                };

                return new UpdatePropertyCommad
                (
                    propertyId,
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
            app.MapPut("properties/{propertyId:guid}", async (
                [FromRoute] Guid propertyId,
                UpdatePropertyRequest request, 
                IMediator mediator, 
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send((UpdatePropertyRequest.ToCommand(request, propertyId)), cancellationToken);
                return result.ToHttpResponse();
            })
            .RequireAuthorization()
            .WithTags(Tags.Properties)
            .WithSummary("Actualiza una propiedad.")
            .WithDescription("Use este endpoint para actualizar todos los valores de una propiedad.");
        }
    }
}

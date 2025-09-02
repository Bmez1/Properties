
using MediatR;

using Microsoft.AspNetCore.Mvc;

using Properties.Api.HttpResponse;
using Properties.Application.UseCases.Properties.Get;

namespace Properties.Api.Endpoints.Properties
{
    public sealed class GetById : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("properties/{propertyId:guid}", async (
                [FromRoute] Guid propertyId, 
                IMediator mediator, 
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new GetByIdQuery(propertyId), cancellationToken);
                return result.ToHttpResponse();
            })
            .RequireAuthorization()
            .WithSummary("Retorna una propiedad por su id.")
            .WithDescription("Retorna una propiedad por su id.")
            .WithTags(Tags.Properties);
        }
    }
}

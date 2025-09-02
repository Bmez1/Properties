
using MediatR;

using Properties.Api.HttpResponse;
using Properties.Application.UseCases.PropertyTraces.List;

namespace Properties.Api.Endpoints.PropertyTraces
{
    public sealed class List : IEndpoint
    {
        public class FilterRequestTraces
        {
            public int? PageNumber { get; init; } = 1;
            public int? PageSize { get; init; } = 10;
        }
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("propertyTraces/{propertyId:guid}", async (
                Guid propertyId,
                [AsParameters] FilterRequestTraces filter,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new ListTracesQuery(propertyId, filter.PageNumber, filter.PageSize), cancellationToken);

                return result.ToHttpResponse();
            })
            .RequireAuthorization()
            .WithSummary("Listar seguimientos de precios de una propiedad")
            .WithDescription("Devuelve una lista paginada de seguimientos de propiedades. Los valores predeterminados de PageNumber y PageSize son 1 y 10.")
            .WithTags(Tags.PropertyTraces);
        }
    }
}

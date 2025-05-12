
using MediatR;

using Properties.Api.HttpResponse;
using Properties.Application.UseCases.PropertyTraces.List;

namespace Properties.Api.Endpoints.PropertyTraces
{
    internal sealed class List : IEndpoint
    {
        public class FilterRequestTraces
        {
            public int PageNumber { get; init; }
            public int PageSize { get; init; }
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
            .WithDescription("List property traces. Returns a paged list of property traces. PageNumber and PageSize default to 1 and 10.")
            .WithTags(Tags.PropertyTraces);
        }
    }
}


using MediatR;

using Properties.Api.HttpResponse;
using Properties.Application.UseCases.Properties.List;

namespace Properties.Api.Endpoints.Properties
{
    public class List : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("properties", async (IMediator mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new ListQuery(), cancellationToken);

                return result.ToHttpResponse(); ;
            })
            .WithTags(Tags.Properties);
        }
    }
}

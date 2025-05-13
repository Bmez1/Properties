using MediatR;

using Properties.Api.HttpResponse;
using Properties.Application.UseCases.Owners.List;

namespace Properties.Api.Endpoints.Owners;

internal sealed class List : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("owners", async (IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new ListQuery(), cancellationToken);

            return result.ToHttpResponse();
        })
        .RequireAuthorization()
        .WithTags(Tags.Owners);
    }
}

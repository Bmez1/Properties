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
        .WithSummary("Retorna la lista de propietarios registrados en el sistema.")
        .WithDescription("Devuelve los datos personales del propietario, la cantidad total de propiedades asociadas y la imagen del propietario si existe.")
        .WithTags(Tags.Owners);
    }
}

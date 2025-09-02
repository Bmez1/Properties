using MediatR;

using Properties.Api.HttpResponse;
using Properties.Application.UseCases.Owners.Get;

namespace Properties.Api.Endpoints.Owners;

public sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("owners/{ownerId:guid}", async (Guid ownerId, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new GetByIdQuery(ownerId), cancellationToken);

            return result.ToHttpResponse();
        })
        .RequireAuthorization()
        .WithSummary("Retorna la información de un propietario registrado en el sistema.")
        .WithDescription("Devuelve los datos personales del propietario, la cantidad total de propiedades asociadas y la imagen del propietario si existe.")
        .WithTags(Tags.Owners);
    }
}


using MediatR;

using Microsoft.AspNetCore.Mvc;

using Properties.Api.HttpResponse;
using Properties.Application.UseCases.Owners.Create;
using Properties.Application.UseCases.Properties.ChangePrice;

using static Properties.Api.Endpoints.Owners.Create;

namespace Properties.Api.Endpoints.Properties
{
    public class ChangePrice : IEndpoint
    {
        public sealed class ChangePriceRequest
        {
            public Guid PropertyId { get; set; }
            public Decimal NewPrice { get; set; }
        }

        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPatch("properties/price", async (ChangePriceRequest request, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var command = new ChangePriceCommand(
                    request.PropertyId,
                    request.NewPrice
                );

                var result = await mediator.Send(command, cancellationToken);

                return result.ToHttpResponse();
            })
            .RequireAuthorization()
            .WithTags(Tags.Properties);
        }
    }
}

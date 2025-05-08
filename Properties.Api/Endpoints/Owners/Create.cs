using MediatR;

using Properties.Api.HttpResponse;
using Properties.Application.UseCases.Owners.Create;

namespace Properties.Api.Endpoints.Owners;

internal sealed class Create : IEndpoint
{
    public sealed class CreateOwnerRequest
    {
        public string Name { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string? Photo { get; set; }
        public DateOnly Birthday { get; set; }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("owners", async (CreateOwnerRequest request, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var command = new CreateOwnerCommand(
                request.Name,
                request.Address,
                request.Birthday,
                request.Photo
            );

            var result = await mediator.Send(command, cancellationToken);

            return result.ToHttpResponse();
        })
        .WithTags("Owners");
    }
}


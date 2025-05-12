using MediatR;

using Microsoft.AspNetCore.Mvc;

using Properties.Api.HttpResponse;
using Properties.Application.UseCases.Owners.Create;
using Properties.Infraestructure.Services;

using static Properties.Api.Endpoints.Properties.AddImage;

namespace Properties.Api.Endpoints.Owners;

internal sealed class Create : IEndpoint
{
    public sealed class CreateOwnerRequest
    {
        public string Name { get; set; } = default!;
        public string Address { get; set; } = default!;
        public IFormFile Photo { get; set; } = default!;
        public DateOnly Birthday { get; set; }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("owners", async ([FromForm] CreateOwnerRequest request, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var fileUpload = request.Photo == null ? null : new FileUpload(request.Photo);
            var command = new CreateOwnerCommand(
                request.Name,
                request.Address,
                request.Birthday,
                fileUpload
            );

            var result = await mediator.Send(command, cancellationToken);

            return result.ToHttpResponse();
        })
        .WithTags(Tags.Owners)
        .Accepts<UploadImageRequest>("multipart/form-data")
        .DisableAntiforgery();
    }
}


using MediatR;

using Microsoft.AspNetCore.Mvc;

using Properties.Api.HttpResponse;
using Properties.Application.UseCases.Properties.AddImage;
using Properties.Infraestructure.Services;

namespace Properties.Api.Endpoints.Properties
{
    public class AddImage : IEndpoint
    {
        public sealed class UploadImageRequest
        {
            public IFormFile Image { get; set; } = default!;
        }
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("properties/{propertyId:guid}/image", async (
                [FromRoute] Guid propertyId,
                [FromForm] UploadImageRequest file,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var fileUpload = new FileUpload(file.Image);
                var command = new AddImageCommand(fileUpload, propertyId);
                var result = await mediator.Send(command, cancellationToken);

                return result.ToHttpResponse();
            })
            .RequireAuthorization()
            .WithTags(Tags.Properties)
            .Accepts<UploadImageRequest>("multipart/form-data")
            .DisableAntiforgery();
        }
    }
}

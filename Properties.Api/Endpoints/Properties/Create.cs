﻿using MediatR;

using Properties.Api.HttpResponse;
using Properties.Application.UseCases.Properties.Create;
using Properties.Application.UseCases.Properties.Dtos;

using System.ComponentModel;

namespace Properties.Api.Endpoints.Properties;

internal sealed class Create : IEndpoint
{
    public sealed class CreatePropertyRequest
    {
        [property: Description("Property name")]
        public string Name { get; set; } = default!;
        [property: Description("Property address")]
        public string Address { get; set; } = default!;
        public decimal Price { get; set; }
        public int Year { get; set; }
        [property: Description("Optional owner id")]
        public Guid? OwnerId { get; set; }
        [property: Description("Optional trace info like property value and tax")]
        public PropertyTraceRequest? Trace { get; set; }
    }

    public class PropertyTraceRequest
    {
        public decimal Value { get; init; }
        public decimal Tax { get; init; }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("properties", async (CreatePropertyRequest request, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var trace = request.Trace is null ? null :
                new PropertyTraceCreateDto
                {
                    Value = request.Trace.Value,
                    Tax = request.Trace.Tax
                };

            var command = new CreatePropertyCommand(
                request.Name,
                request.Address,
                request.Price,
                request.Year,
                request.OwnerId,
                trace!
            );

            var result = await mediator.Send(command, cancellationToken);

            return result.ToHttpResponse(); ;
        })
        .RequireAuthorization()
        .WithTags(Tags.Properties)
        .WithSummary("Creates a new property.")
        .WithDescription("Creates a new property, optionally assigning an owner and property trace.");
    }
}


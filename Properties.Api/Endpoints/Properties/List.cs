﻿
using MediatR;

using Properties.Api.HttpResponse;
using Properties.Application.UseCases.Properties.Dtos;
using Properties.Application.UseCases.Properties.List;

namespace Properties.Api.Endpoints.Properties
{
    public class List : IEndpoint
    {
        public class RequestFilterDto
        {
            public string? Name { get; init; }
            public string? Address { get; init; }
            public decimal? MinPrice { get; init; }
            public decimal? MaxPrice { get; init; }
            public int? MinYear { get; init; }
            public int? MaxYear { get; init; }
            public string? OwnerName { get; init; }

            public int? PageNumber { get; init; } = 1;
            public int? PageSize { get; init; } = 10;


            public static implicit operator PropertyFilterDto(RequestFilterDto dto)
            {
                return new PropertyFilterDto
                {
                    Name = dto.Name,
                    Address = dto.Address,
                    MinPrice = dto.MinPrice,
                    MaxPrice = dto.MaxPrice,
                    MinYear = dto.MinYear,
                    MaxYear = dto.MaxYear,
                    OwnerName = dto.OwnerName,
                    PageNumber = dto.PageNumber,
                    PageSize = dto.PageSize
                };
            }

        }

        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("properties", async (
                [AsParameters] RequestFilterDto filter,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new ListQuery(filter), cancellationToken);

                return result.ToHttpResponse();
            })
            .RequireAuthorization()
            .WithDescription("Returns a list of properties. PageNumber and PageSize default to 1 and 10.")
            .WithTags(Tags.Properties);
        }
    }
}

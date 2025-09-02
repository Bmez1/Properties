using Properties.Application.UseCases.Properties.Dtos;
using Properties.Domain.Entities;

namespace Properties.Application.UseCases.Properties.Mappers
{
    public static class PropertyMapper
    {
        public static CreatePropertyResponseDto MapToCreatePropertyResponseDto(this Property property)
        {
            return new CreatePropertyResponseDto
            {
                PropertyId = property.Id,
                Name = property.Name,
                Address = property.Address,
                CodeInternal = property.CodeInternal,
                Year = property.Year,
                Price = property.Price
            };
        }
    }
}

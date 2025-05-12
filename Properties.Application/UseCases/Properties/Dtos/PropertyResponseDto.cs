namespace Properties.Application.UseCases.Properties.Dtos
{
    public class PropertyResponseDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = default!;
        public string Address { get; init; } = default!;
        public decimal Price { get; init; }
        public int Year { get; init; }
        public string CodeInternal { get; init; } = default!;
        public bool IsAvailable { get; init; } = default!;
        public Guid? OwnerId { get; init; }
        public string? OwnerName { get; init; }
        public required int ImagesCount { get; init; }
    }
}

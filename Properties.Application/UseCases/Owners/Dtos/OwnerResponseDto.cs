namespace Properties.Application.UseCases.Owners.Dtos
{
    public class OwnerResponseDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = default!;
        public string Address { get; init; } = default!;
        public string? Photo { get; init; } = default!;
        public DateOnly Birthday { get; init; }
        public int NumberProperties { get; init; }
    }
}

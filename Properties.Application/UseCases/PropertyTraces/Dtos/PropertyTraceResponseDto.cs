namespace Properties.Application.UseCases.PropertyTraces.Dtos
{
    public class PropertyTraceResponseDto
    {
        public Guid Id { get; init; }
        public DateTime DateSale { get; init; }
        public string Name { get; init; } = default!;
        public decimal Value { get; init; }
        public decimal Tax { get; init; }
        public Guid PropertyId { get; init; }
    }
}

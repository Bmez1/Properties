namespace Properties.Application.UseCases.Properties.Dtos
{
    public class PropertyTraceCreateDto
    {
        public DateTime DateSale { get; init; }
        public string Name { get; init; } = default!;
        public decimal Value { get; init; }
        public decimal Tax { get; init; }
    }
}

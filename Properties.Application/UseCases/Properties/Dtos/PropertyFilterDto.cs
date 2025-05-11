namespace Properties.Application.UseCases.Properties.Dtos
{
    public class PropertyFilterDto
    {
        public string? Name { get; init; }
        public string? Address { get; init; }
        public decimal? MinPrice { get; init; }
        public decimal? MaxPrice { get; init; }
        public int? MinYear { get; init; }
        public int? MaxYear { get; init; }
        public string? OwnerName { get; init; }
        public int? PageNumber { get; init; }
        public int? PageSize { get; init; }
    }
}

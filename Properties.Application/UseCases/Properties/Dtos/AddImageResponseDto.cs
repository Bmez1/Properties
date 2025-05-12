namespace Properties.Application.UseCases.Properties.Dtos
{
    public class AddImageResponseDto
    {
        public Guid PropertyId { get; init; }
        public string Image { get; init; } = default!;
    }
}

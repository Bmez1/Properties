namespace Properties.Domain.Entities
{
    public class PropertyTrace : EntityBase
    {
        public DateOnly DateSale { get; set; }
        public string Name { get; set; } = default!;
        public decimal Value { get; set; }
        public decimal Tax { get; set; }

        public Guid PropertyId { get; set; }
        public Property Property { get; set; } = default!;


        public PropertyTrace(Guid id, Guid propertyId, string name, DateOnly dateSale, decimal value, decimal tax, DateTime createdAt)
        {
            Id = id;
            PropertyId = propertyId;
            Name = name;
            DateSale = dateSale;
            Value = value;
            Tax = tax;
            CreatedAt = createdAt;
        }

        public static PropertyTrace Create(Guid propertyId, string name, DateOnly dateSale, decimal value, decimal tax) => new (Guid.NewGuid(), propertyId, name, dateSale, value, tax, DateTime.UtcNow);

    }
}

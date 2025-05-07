namespace Properties.Domain.Entities
{
    public class PropertyTrace : EntityBase
    {
        public DateOnly DateSale { get; private set; }
        public string Name { get; private set; } = default!;
        public decimal Value { get; private set; }
        public decimal Tax { get; private set; }

        public Guid PropertyId { get; private set; }
        public Property Property { get; private set; } = default!;


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

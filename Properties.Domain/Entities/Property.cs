using Crosscutting;

namespace Properties.Domain.Entities
{
    public class Property : EntityBase
    {
        public const string NameDirectory = "Properties";
        public string Name { get; private set; } = default!;
        public string Address { get; private set; } = default!;
        public decimal Price { get; private set; }
        public string CodeInternal { get; private set; } = default!;
        public int Year { get; private set; }

        public Guid? OwnerId { get; private set; }
        public Owner Owner { get; private set; } = default!;

        public ICollection<PropertyImage> Images { get; private set; } = [];
        public ICollection<PropertyTrace> Traces { get; private set; } = [];

        private Property(Guid id, string name, string address, decimal price, string codeInternal, int year, Guid? ownerId, DateTime createdAt)
        {
            Id = id;
            Name = name;
            Address = address;
            Price = price;
            CodeInternal = codeInternal;
            Year = year;
            OwnerId = ownerId;
            CreatedAt = createdAt;
        }

        public static Property Create(string name, string address, decimal price, int year, Guid? ownerId = null) =>
            new(Guid.NewGuid(), name, address, price, Guid.NewGuid().ToString(), year, ownerId, DateTime.UtcNow);

        public void AddImage(string file)
        {
            Images.Add(PropertyImage.Create(file));
        }

        public void AddTrace(string name, DateOnly dateSale, decimal value, decimal tax)
        {
            Traces.Add(PropertyTrace.Create(Id, name, dateSale, value, tax));
        }

        public void UpdatePrice(decimal newPrice)
        {
            if (newPrice <= 0) throw new ArgumentException("El precio debe ser mayor a cero.");
            Price = newPrice;
        }

    }
}

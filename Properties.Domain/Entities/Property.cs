using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;

namespace Properties.Domain.Entities
{
    public class Property : EntityBase
    {
        public string Name { get; set; } = default!;
        public string Address { get; set; } = default!;
        public decimal Price { get; set; }
        public string CodeInternal { get; set; } = default!;
        public int Year { get; set; }

        public Guid OwnerId { get; set; }
        public Owner Owner { get; set; } = default!;

        public ICollection<PropertyImage> Images { get; private set; } = [];
        public ICollection<PropertyTrace> Traces { get; private set; } = [];

        private Property(Guid id, string name, string address, decimal price, string codeInternal, int year, Guid ownerId, DateTime createdAt)
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

        public static Property Create(string name, string address, decimal price, string codeInternal, int year, Guid ownerId, DateTime createdAt) =>
            new (Guid.NewGuid(), name, address, price, codeInternal, year, ownerId, createdAt);

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

using Crosscutting;

namespace Properties.Domain.Entities
{
    public class Owner : EntityBase
    {
        public const string Directory = "Owners";
        public string Name { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string? Photo { get; set; }
        public DateOnly Birthday { get; set; }

        public ICollection<Property> Properties { get; private set; } = [];

        private Owner(Guid id, string name, string address, DateOnly birthday, DateTime createdAt, string? photo = null)
        {
            Id = id;
            Name = name;
            Address = address;
            Birthday = birthday;
            Photo = photo;
            CreatedAt = createdAt;
        }

        public static Owner Create(string name, string address, DateOnly birthday, string? photo = null)
        {
            return new Owner(Guid.NewGuid(), name, address, birthday, DateTime.UtcNow, photo);
        }

        public void AddProperty(Property property)
        {
            ArgumentNullException.ThrowIfNull(property);
            Properties.Add(property);
        }

    }
}

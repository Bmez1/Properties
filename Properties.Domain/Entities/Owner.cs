using Crosscutting;

namespace Properties.Domain.Entities
{
    public class Owner : EntityBase
    {
        public const string Directory = "Owners";
        public string Name { get; private set; }
        public string Address { get; private set; }
        public string? Photo { get; private set; }
        public DateOnly Birthday { get; private set; }

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

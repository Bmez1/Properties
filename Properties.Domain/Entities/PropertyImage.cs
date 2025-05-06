namespace Properties.Domain.Entities
{
    public class PropertyImage : EntityBase
    {
        public string File { get; set; } = default!;
        public bool Enabled { get; set; }

        public Guid PropertyId { get; set; }
        public Property Property { get; set; } = default!;

        public PropertyImage(string file, bool enabled, Guid propertyId, DateTime createdAt)
        {
            File = file;
            Enabled = enabled;
            PropertyId = propertyId;
            CreatedAt = createdAt;
        }

        public static PropertyImage Create(string file) => new (file, true, Guid.NewGuid(), DateTime.UtcNow);

        public void Enable() => Enabled = true;
        public void Disable() => Enabled = false;
    }
}

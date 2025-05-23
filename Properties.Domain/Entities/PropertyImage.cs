﻿using Crosscutting;

namespace Properties.Domain.Entities
{
    public class PropertyImage : EntityBase
    {
        public string File { get; private set; } = default!;
        public bool Enabled { get; private set; } = true;

        public Guid PropertyId { get; private set; }
        public Property Property { get; private set; } = default!;

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

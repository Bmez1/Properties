namespace Properties.Domain.Entities
{
    public abstract class EntityBase
    {
        public Guid Id { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
    }
}

namespace Crosscutting;

public abstract class EntityBase
{
    public Guid Id { get; protected set; }
    public DateTime CreatedAt { get; protected set; }
}

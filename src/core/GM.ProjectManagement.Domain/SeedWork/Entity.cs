namespace GM.ProjectManagement.Domain.SeedWork;

public abstract class Entity
{
    protected Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; private set; } = DateTime.Now;
    public DateTime UpdatedAt { get; private set; } = DateTime.Now;
}

namespace GM.ProjectManagement.Domain.SeedWork;

public abstract class Entity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime CreatedAt { get; private set; } = DateTime.Now;

    public DateTime? UpdatedAt { get; private set; }

    public void SetUpdatedAt(DateTime updatedAt)
    {
        UpdatedAt = updatedAt;
    }
}

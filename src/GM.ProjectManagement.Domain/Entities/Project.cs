using GM.ProjectManagement.Domain.SeedWork;

namespace GM.ProjectManagement.Domain.Entities;

public class Project : Entity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public ICollection<ProjectTask> Tasks { get; set; } = [];
}

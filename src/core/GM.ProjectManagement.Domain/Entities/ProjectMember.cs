using GM.ProjectManagement.Domain.Enums;
using GM.ProjectManagement.Domain.SeedWork;
using GM.ProjectManagement.Domain.Validations;

namespace GM.ProjectManagement.Domain.Entities;

public class ProjectMember : Entity
{
    public bool IsActive { get; private set; } = true;

    public ProjectMemberRole Role { get; private set; }

    public  Guid UserId { get; private set; }

    public virtual IReadOnlyList<Project> Projects => _projects.AsReadOnly();

    public virtual IReadOnlyList<ProjectTask> Tasks => _tasks.AsReadOnly();

    private readonly IList<Project> _projects = [];

    private readonly IList<ProjectTask> _tasks = [];

    public ProjectMember(ProjectMemberRole role, Guid userId)
    {
        Role = role;
        UserId = userId;

        Validate();
    }

    private void Validate()
    {
        DomainValidation.NotNull(Role, nameof(Role));
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void ChangeRole(ProjectMemberRole role)
    {
        DomainValidation.NotNull(role, nameof(role));

        Role = role;
    }
}

using GM.ProjectManagement.Domain.Enums;
using GM.ProjectManagement.Domain.SeedWork;
using GM.ProjectManagement.Domain.Validations;

namespace GM.ProjectManagement.Domain.Entities;

public class ProjectMember : Entity
{
    public string Name { get; private set; }

    public string SurName { get; private set; }

    public bool IsActive { get; private set; } = true;

    public ProjectMemberRole Role { get; private set; }

    public  Guid UserId { get; private set; }

    public User User { get; private set; }

    public virtual IReadOnlyList<Project> Projects => _projects.AsReadOnly();

    public virtual IReadOnlyList<ProjectTask> Tasks => _tasks.AsReadOnly();

    private readonly IList<Project> _projects = [];

    private readonly IList<ProjectTask> _tasks = [];

    public ProjectMember(string name, string surName, ProjectMemberRole role, User user)
    {
        Name = name;
        SurName = surName;
        Role = role;
        User = user;

        Validate();
    }

    private void Validate()
    {
        DomainValidation.NotNullOrEmpty(Name, nameof(Name));
        DomainValidation.MinLength(Name, ValidationConstants.ProjectMember_MinNameLength, nameof(Name));
        DomainValidation.MaxLength(Name, ValidationConstants.ProjectMember_MaxNameLength, nameof(Name));

        DomainValidation.NotNullOrEmpty(SurName, nameof(SurName));
        DomainValidation.MinLength(SurName, ValidationConstants.ProjectMember_MinSurNameLength, nameof(SurName));
        DomainValidation.MaxLength(SurName, ValidationConstants.ProjectMember_MaxSurNameLength, nameof(SurName));

        DomainValidation.NotNull(Role, nameof(Role));

        DomainValidation.NotNull(User, nameof(User));
    }

    public void Update(string name, string surname)
    {
        Name = name;
        SurName = surname;

        Validate();
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

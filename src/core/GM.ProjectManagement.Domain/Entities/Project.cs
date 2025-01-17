using GM.ProjectManagement.Domain.Entities.ValueObjects;
using GM.ProjectManagement.Domain.Enums;
using GM.ProjectManagement.Domain.Exceptions;
using GM.ProjectManagement.Domain.SeedWork;
using GM.ProjectManagement.Domain.Validations;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GM.ProjectManagement.Domain.Entities;

public class Project : Entity
{
    public string Name { get; private set; }

    public string Description { get; private set; }

    public DateOnly StartDate { get; private set; }

    public DateOnly? Deadline { get; private set; }

    public DateOnly? CompletionDate { get; private set; }

    public ProjectStatus Status { get; private set; } = ProjectStatus.Created;

    public Guid ProjectOwnerId { get; private set; }

    public virtual ProjectOwner ProjectOwner { get; private set; }

    public virtual IReadOnlyList<ProjectTask> Tasks => _tasks.AsReadOnly();

    public virtual IReadOnlyList<ProjectMember> ProjectMembers => _ProjectMembers.AsReadOnly();

    private readonly IList<ProjectTask> _tasks = [];

    private readonly IList<ProjectMember> _ProjectMembers = [];

    public Project(string name, string description, ProjectOwner projectOwner, DateOnly startDate, DateOnly? deadline = null)
    {
        Name = name;
        Description = description;
        StartDate = startDate;
        ProjectOwner = projectOwner;

        if (deadline.HasValue)
        {
            SetDeadline(deadline.Value);
        }

        Validate();
    }

    private void Validate()
    {
        DomainValidation.NotNullOrEmpty(Name, nameof(Name));
        DomainValidation.MinLength(Name, ValidationConstants.Project_MinNameLength, nameof(Name));
        DomainValidation.MaxLength(Name, ValidationConstants.Project_MaxNameLength, nameof(Name));

        DomainValidation.NotNullOrEmpty(Description, nameof(Description));
        DomainValidation.MinLength(Description, ValidationConstants.Project_MinDescriptionLength, nameof(Description));

        DomainValidation.NotBeforeCurrentDate(StartDate, nameof(StartDate));

        DomainValidation.NotNull(ProjectOwner, nameof(ProjectOwner));
    }

    public void SetProjectOwner(ProjectOwner projectOwner)
    {
        ValidateIfProjectCanBeUpdated();

        ProjectOwner = projectOwner;

        Validate();
    }

    public void Update(string name, string description)
    {
        ValidateIfProjectCanBeUpdated();

        Name = name;
        Description = description;

        Validate();
    }

    public void AddTask(string title, string description, DateOnly? dueDate = null)
    {
        ValidateIfProjectCanBeUpdated();

        var task = ProjectTask.Create(title, description, dueDate);

        _tasks.Add(task);
    }

    public void RemoveTask(ProjectTask task)
    {
        DomainValidation.NotNull(task, nameof(task));

        ValidateIfProjectCanBeUpdated();

        if (!_tasks.Contains(task))
        {
            throw new EntityValidationException($"Task {task.Title} does not exists in this project");
        }

        _tasks.Remove(task);
    }

    public void AddProjectMember(ProjectMember projectMember)
    {
        DomainValidation.NotNull(projectMember, nameof(ProjectMember));

        ValidateIfProjectCanBeUpdated();

        if (HasProjectMember(projectMember))
            throw new EntityValidationException($"ProjectMember {projectMember.Name} is already member of this project");

        if (ProjectMembers.Count == ValidationConstants.Project_MaxProjectMembers)
            throw new EntityValidationException($"Project {Name} has already reached the maximum number of members");

        _ProjectMembers.Add(projectMember);
    }

    public void RemoveProjectMember(ProjectMember projectMember)
    {
        DomainValidation.NotNull(projectMember, nameof(projectMember));

        ValidateIfProjectCanBeUpdated();

        if (!HasProjectMember(projectMember))
            throw new EntityValidationException($"ProjectMember {projectMember.Name} does not is member of this project");

        if (Tasks.Any(t => t.ProjectMemberId == projectMember.Id))
            throw new EntityValidationException($"The ProjectMember {projectMember.Name} cannot be removed because has one or more related tasks");

        _ProjectMembers.Remove(projectMember);
    }

    public void ChangeStatus(ProjectStatus newStatus)
    {
        ValidateIfProjectCanBeUpdated();

        if (!IsValidTransitionStatus(Status, newStatus))
            throw new EntityValidationException($"Invalid project status transition from {Status} to {newStatus}");

        Status = newStatus;

        if (Status == ProjectStatus.Completed)
            CompletionDate = DateOnly.FromDateTime(DateTime.UtcNow);
    }

    public void SetDeadline(DateOnly deadline)
    {
        ValidateIfProjectCanBeUpdated();

        ValidateDeadline(deadline, StartDate, nameof(deadline));

        Deadline = deadline;
    }

    private static bool IsValidTransitionStatus(ProjectStatus current, ProjectStatus newStatus)
    {
        return (ValidTransitions.ForProjectStatus().Contains((current, newStatus)));
    }

    private void ValidateIfProjectCanBeUpdated()
    {
        if (Status == ProjectStatus.Completed)
            throw new EntityValidationException("Completed project cannot to be changed");
    }

    private bool HasProjectMember(ProjectMember projectMember)
    {
        return _ProjectMembers.Any(c => c.Id == projectMember.Id);
    }

    private static void ValidateDeadline(DateOnly? deadline, DateOnly startDate, string fieldName)
    {
        DomainValidation.NotNull(deadline, fieldName);
        DomainValidation.NotBeforeCurrentDate(deadline, fieldName);

        if (deadline < startDate)
            throw new EntityValidationException($"Project deadline must be after the start date");
    }
}

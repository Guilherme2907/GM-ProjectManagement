using GM.ProjectManagement.Domain.Enums;
using GM.ProjectManagement.Domain.Exceptions;
using GM.ProjectManagement.Domain.SeedWork;
using GM.ProjectManagement.Domain.Validations;

namespace GM.ProjectManagement.Domain.Entities;

public class ProjectTask : Entity
{
    public string Title { get; private set; }

    public string Description { get; private set; }

    public ProjectTaskStatus Status { get; private set; } = ProjectTaskStatus.Backlog;

    public DateOnly? DueDate { get; private set; }

    public Guid ProjectId { get; private set; }

    public Project Project { get; private set; }

    public Guid? ProjectMemberId { get; set; }

    public ProjectMember? ProjectMember { get; private set; }

    private ProjectTask(string title, string description, DateOnly? dueDate)
    {
        Title = title;
        Description = description;
        DueDate = dueDate;

        if (dueDate.HasValue)
        {
            SetDueDate(dueDate.Value);
        }

        Validate();
    }

    internal static ProjectTask Create(string title, string description, DateOnly? dueDate)
    {
        return new ProjectTask(title, description, dueDate);
    }

    private void Validate()
    {
        DomainValidation.NotNullOrEmpty(Title, nameof(Title));
        DomainValidation.MinLength(Title, ValidationConstants.Task_MinTitleLength, nameof(Title));
        DomainValidation.MaxLength(Title, ValidationConstants.Task_MaxTitleLength, nameof(Title));

        DomainValidation.NotNullOrEmpty(Description, nameof(Description));
        DomainValidation.MinLength(Description, ValidationConstants.Task_MinDescriptionLength, nameof(Description));
    }

    public void Update(string title, string description)
    {
        Title = title;
        Description = description;

        Validate();
    }

    public void SetDueDate(DateOnly dueDate)
    {
        DomainValidation.NotBeforeCurrentDate(dueDate, nameof(dueDate));

        DueDate = dueDate;
    }

    public void SetProjectMember(ProjectMember? projectMember)
    {
        ProjectMember = projectMember;
        ProjectMemberId = ProjectMember?.Id;
    }

    public void ChangeStatus(ProjectTaskStatus newStatus)
    {
        if(!IsValidTransitionStatus(Status, newStatus))
            throw new EntityValidationException($"Invalid task status transition from {Status} to {newStatus}");

        Status = newStatus;
    }

    private static bool IsValidTransitionStatus(ProjectTaskStatus current, ProjectTaskStatus newStatus)
    {
        return ValidTransitions.ForProjectTaskStatus().Contains((current, newStatus));
    }
}

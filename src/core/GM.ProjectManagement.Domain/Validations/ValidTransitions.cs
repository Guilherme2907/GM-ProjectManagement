using GM.ProjectManagement.Domain.Enums;

namespace GM.ProjectManagement.Domain.Validations;

public static class ValidTransitions
{
    public static HashSet<(ProjectStatus before, ProjectStatus after)> ForProjectStatus()
    {
        return [
            (ProjectStatus.Created, ProjectStatus.InProgress),
            (ProjectStatus.Created, ProjectStatus.Canceled),
            (ProjectStatus.InProgress, ProjectStatus.Canceled),
            (ProjectStatus.InProgress, ProjectStatus.Completed)
        ];
    }

    public static HashSet<(ProjectTaskStatus before, ProjectTaskStatus after)> ForProjectTaskStatus()
    {
        return [
            (ProjectTaskStatus.Backlog, ProjectTaskStatus.Approved),
            (ProjectTaskStatus.Approved, ProjectTaskStatus.InProgress),
            (ProjectTaskStatus.InProgress, ProjectTaskStatus.Validation),
            (ProjectTaskStatus.Validation, ProjectTaskStatus.Completed)
        ];
    }
}

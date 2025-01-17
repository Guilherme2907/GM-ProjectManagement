namespace GM.ProjectManagement.Domain.Validations;

public static class ValidationConstants
{
    public const int Project_MaxProjectMembers = 20;
    public const int Project_MinNameLength = 3;
    public const int Project_MaxNameLength = 30;
    public const int Project_MinDescriptionLength = 10;

    public const int ProjectMember_MinNameLength = 3;
    public const int ProjectMember_MinSurNameLength = 3;
    public const int ProjectMember_MaxNameLength = 15;
    public const int ProjectMember_MaxSurNameLength = 15;

    public const int Task_MinTitleLength = 5;
    public const int Task_MaxTitleLength = 20;
    public const int Task_MinDescriptionLength = 20;
}

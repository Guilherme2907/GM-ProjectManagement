using GM.ProjectManagement.Domain.Enums;
using GM.ProjectManagement.Domain.Exceptions;
using GM.ProjectManagement.Domain.Validations;

namespace GM.ProjectManagement.Domain.Entities.ValueObjects;

public class ProjectOwner
{
    public ProjectMember Owner { get; private set; }

    public ProjectOwner(ProjectMember owner)
    {
        DomainValidation.NotNull(owner, nameof(Owner));

        if (!IsAValidProjectOwner(owner))
            throw new EntityValidationException($"Only admins and managers can be project owners");

        Owner = owner;
    }

    private static bool IsAValidProjectOwner(ProjectMember owner)
    {
        return owner.Role == ProjectMemberRole.Manager || owner.Role == ProjectMemberRole.Admin;
    }
}

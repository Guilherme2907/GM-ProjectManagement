using FluentAssertions;
using GM.ProjectManagement.Domain.Enums;
using GM.ProjectManagement.Domain.Exceptions;
using GM.ProjectManagement.Domain.Validations;
using Entity = GM.ProjectManagement.Domain.Entities;

namespace GM.ProjectManagement.UnitTests.Domain.Entities.Collaborator;

[Collection(nameof(ProjectMemberTestFixture))]
public class ProjectMemberTest(ProjectMemberTestFixture fixture)
{
    private readonly ProjectMemberTestFixture _fixture = fixture;

    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "ProjectMember - Aggregates")]
    public void Instantiate()
    {
        var role = _fixture.GetRandomStatusValue<ProjectMemberRole>();

        var projectMember = new Entity.ProjectMember(role, Guid.NewGuid());

        projectMember.Should().NotBeNull();
        projectMember.Role.Should().Be(role);
    }

    [Fact(DisplayName = nameof(Activate))]
    [Trait("Domain", "ProjectMember - Aggregates")]
    public void Activate()
    {
        var projectMember = _fixture.GetProjectMember();

        projectMember.Activate();

        projectMember.IsActive.Should().BeTrue();
    }

    [Fact(DisplayName = nameof(Deactivate))]
    [Trait("Domain", "ProjectMember - Aggregates")]
    public void Deactivate()
    {
        var projectMember = _fixture.GetProjectMember();

        projectMember.Deactivate();

        projectMember.IsActive.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(ChangeRole))]
    [Trait("Domain", "ProjectMember - Aggregates")]
    public void ChangeRole()
    {
        var projectMember = _fixture.GetProjectMember();
        var newRole = _fixture.GetRandomStatusValue<ProjectMemberRole>();

        projectMember.ChangeRole(newRole);

        projectMember.Role.Should().Be(newRole);
    }
}

using Bogus;
using FluentAssertions;
using GM.ProjectManagement.Domain.Entities;
using GM.ProjectManagement.Domain.Enums;
using GM.ProjectManagement.Domain.Exceptions;
using GM.ProjectManagement.Domain.SeedWork;
using VO = GM.ProjectManagement.Domain.Entities.ValueObjects;

namespace GM.ProjectManagement.UnitTests.Domain.ValueObjects.ProjectOwner;

[Collection(nameof(ProjectOwnerTestFixture))]
public class ProjectOwnerTest(ProjectOwnerTestFixture fixture)
{
    private readonly ProjectOwnerTestFixture _fixture = fixture;

    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "ProjectOwner - ValueObjects")]
    public void Instantiate()
    {
        var projectMember = _fixture.GetProjectMember(true);

        var projectOwner = new VO.ProjectOwner(projectMember);

        projectOwner.Should().NotBeNull();
        projectOwner.Owner.Should().Be(projectMember);
    }

    [Fact(DisplayName = nameof(InstantiateErroWhenOwnerIsNull))]
    [Trait("Domain", "ProjectOwner - ValueObjects")]
    public void InstantiateErroWhenOwnerIsNull()
    {
        Action action =
                () => new VO.ProjectOwner(null!);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Field Owner cannot be null");
    }

    [Fact(DisplayName = nameof(InstantiateErroWhenOwnerIsNotValid))]
    [Trait("Domain", "ProjectOwner - ValueObjects")]
    public void InstantiateErroWhenOwnerIsNotValid()
    {
        var ProjectMember = _fixture.GetProjectMember();
        ProjectMember.ChangeRole(ProjectMemberRole.Member);

        Action action =
                () => new VO.ProjectOwner(ProjectMember);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Only admins and managers can be project owners");
    }
}

using GM.ProjectManagement.UnitTests.Common;

namespace GM.ProjectManagement.UnitTests.Domain.Entities.ProjectMember;

[CollectionDefinition(nameof(ProjectMemberTestFixture))]
public class ProjectMemberTestFixtureCollection : ICollectionFixture<ProjectMemberTestFixture>
{ }

public class ProjectMemberTestFixture : BaseFixture
{
}

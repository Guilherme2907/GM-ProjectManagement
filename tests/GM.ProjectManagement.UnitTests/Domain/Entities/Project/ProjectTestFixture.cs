using GM.ProjectManagement.UnitTests.Common;

namespace GM.ProjectManagement.UnitTests.Domain.Entities.Project;

[CollectionDefinition(nameof(ProjectTestFixture))]
public class ProjectTestFixtureCollection : ICollectionFixture<ProjectTestFixture>
{ }

public class ProjectTestFixture : BaseFixture
{

}

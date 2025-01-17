using GM.ProjectManagement.UnitTests.Common;

namespace GM.ProjectManagement.UnitTests.Domain.Entities.ProjectTask;

[CollectionDefinition(nameof(ProjectTaskTestFixture))]
public class ProjectTaskTestFixtureCollection : ICollectionFixture<ProjectTaskTestFixture>
{ }

public class ProjectTaskTestFixture : BaseFixture
{
}

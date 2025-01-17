using FluentAssertions;
using GM.ProjectManagement.Domain.Enums;
using GM.ProjectManagement.Domain.Exceptions;
using GM.ProjectManagement.Domain.Validations;
using Entity = GM.ProjectManagement.Domain.Entities;

namespace GM.ProjectManagement.UnitTests.Domain.Entities.ProjectMember;

[Collection(nameof(ProjectMemberTestFixture))]
public class ProjectMemberTest(ProjectMemberTestFixture fixture)
{
    private readonly ProjectMemberTestFixture _fixture = fixture;

    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "ProjectMember - Aggregates")]
    public void Instantiate()
    {
        var name = _fixture.GetValidProjectMemberName();
        var surname = _fixture.GetValidProjectMemberSurname();
        var role = _fixture.GetRandomStatusValue<ProjectMemberRole>();
        var user = _fixture.GetUser();

        var projectMember = new Entity.ProjectMember(name, surname, role, user);

        projectMember.Should().NotBeNull();
        projectMember.Name.Should().Be(name);
        projectMember.SurName.Should().Be(surname);
        projectMember.User.Should().Be(user);
        projectMember.Role.Should().Be(role);
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsNullOrEmpty))]
    [Trait("Domain", "ProjectMember - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    public void InstantiateErrorWhenNameIsNullOrEmpty(string invalidName)
    {
        Action action =
        () => new Entity.ProjectMember(invalidName, _fixture.GetValidProjectMemberSurname(), _fixture.GetRandomStatusValue<ProjectMemberRole>(), _fixture.GetUser());

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Field Name cannot be null or empty");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenNameHasLessThanMinimumCharacteres))]
    [Trait("Domain", "ProjectMember - Aggregates")]
    public void InstantiateErrorWhenNameHasLessThanMinimumCharacteres()
    {
        var invalidName = _fixture.GetValidProjectMemberName()[..(ValidationConstants.ProjectMember_MinNameLength - 1)];

        Action action =
            () => new Entity.ProjectMember(invalidName, _fixture.GetValidProjectMemberSurname(), _fixture.GetRandomStatusValue<ProjectMemberRole>(), _fixture.GetUser());

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"Name should be at least {ValidationConstants.ProjectMember_MinNameLength} characters long");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenNameHasMoreThanMaximumCharacteres))]
    [Trait("Domain", "ProjectMember - Aggregates")]
    public void InstantiateErrorWhenNameHasMoreThanMaximumCharacteres()
    {
        var invalidName = string.Empty;

        while (invalidName.Length <= ValidationConstants.ProjectMember_MaxNameLength)
        {
            invalidName += _fixture.GetValidProjectMemberName();
        }

        Action action =
            () => new Entity.ProjectMember(invalidName, _fixture.GetValidProjectMemberSurname(), _fixture.GetRandomStatusValue<ProjectMemberRole>(), _fixture.GetUser());

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"Name should be less or equal {ValidationConstants.ProjectMember_MaxNameLength} characters long");
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenSurnameIsNullOrEmpty))]
    [Trait("Domain", "ProjectMember - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    public void InstantiateErrorWhenSurnameIsNullOrEmpty(string invalidSurname)
    {
        Action action =
        () => new Entity.ProjectMember(_fixture.GetValidProjectMemberName(), invalidSurname, _fixture.GetRandomStatusValue<ProjectMemberRole>(), _fixture.GetUser());

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Field Surname cannot be null or empty");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenSurnameHasLessThanMinimumCharacteres))]
    [Trait("Domain", "ProjectMember - Aggregates")]
    public void InstantiateErrorWhenSurnameHasLessThanMinimumCharacteres()
    {
        var invalidSurname = _fixture.GetValidProjectMemberName()[..(ValidationConstants.ProjectMember_MinSurNameLength - 1)];

        Action action =
            () => new Entity.ProjectMember(_fixture.GetValidProjectMemberName(), invalidSurname, _fixture.GetRandomStatusValue<ProjectMemberRole>(), _fixture.GetUser());

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"Surname should be at least {ValidationConstants.ProjectMember_MinSurNameLength} characters long");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenSurnameHasMoreThanMaximumCharacteres))]
    [Trait("Domain", "ProjectMember - Aggregates")]
    public void InstantiateErrorWhenSurnameHasMoreThanMaximumCharacteres()
    {
        var invalidSurname = string.Empty;

        while (invalidSurname.Length <= ValidationConstants.ProjectMember_MaxSurNameLength)
        {
            invalidSurname += _fixture.GetValidProjectMemberName();
        }

        Action action =
            () => new Entity.ProjectMember(_fixture.GetValidProjectMemberName(), invalidSurname, _fixture.GetRandomStatusValue<ProjectMemberRole>(), _fixture.GetUser());

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"Surname should be less or equal {ValidationConstants.ProjectMember_MaxSurNameLength} characters long");
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "ProjectMember - Aggregates")]
    public void Update()
    {
        var projectMember = _fixture.GetProjectMember();
        var newProjectMemberName = _fixture.GetValidProjectMemberName();
        var newProjectMemberSurname = _fixture.GetValidProjectMemberSurname();

        projectMember.Update(newProjectMemberName, newProjectMemberSurname);

        projectMember.Should().NotBeNull();
        projectMember.Name.Should().Be(newProjectMemberName);
        projectMember.SurName.Should().Be(newProjectMemberSurname);
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsNullOrEmpty))]
    [Trait("Domain", "ProjectMember - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    public void UpdateErrorWhenNameIsNullOrEmpty(string projectMemberName)
    {
        var projectMember = _fixture.GetProjectMember();
        var newProjectMemberSurname = _fixture.GetValidProjectMemberSurname();

        Action action =
            () => projectMember.Update(projectMemberName, newProjectMemberSurname);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Field Name cannot be null or empty");
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenSurnameIsNullOrEmpty))]
    [Trait("Domain", "ProjectMember - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    public void UpdateErrorWhenSurnameIsNullOrEmpty(string projectMemberSurname)
    {
        var projectMember = _fixture.GetProjectMember();
        var newProjectMemberName = _fixture.GetValidProjectMemberName();

        Action action =
            () => projectMember.Update(newProjectMemberName, projectMemberSurname);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Field Surname cannot be null or empty");
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

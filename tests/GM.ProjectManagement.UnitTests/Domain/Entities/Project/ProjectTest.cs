using FluentAssertions;
using GM.ProjectManagement.Domain.Enums;
using GM.ProjectManagement.Domain.Exceptions;
using GM.ProjectManagement.Domain.Validations;
using Entity = GM.ProjectManagement.Domain.Entities;

namespace GM.ProjectManagement.UnitTests.Domain.Entities.Project;

[Collection(nameof(ProjectTestFixture))]
public class ProjectTest(ProjectTestFixture fixture)
{
    private readonly ProjectTestFixture _fixture = fixture;

    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Project - Aggregates")]
    public void Instantiate()
    {
        var projectName = _fixture.GetValidProjectName();
        var projectDescription = _fixture.GetValidProjectDescription();
        var projectOwner = _fixture.GetProjectOwner();
        var project = new Entity.Project(projectName, projectDescription, projectOwner, DateOnly.FromDateTime(DateTime.Now));

        project.Should().NotBeNull();
        project.Name.Should().Be(projectName);
        project.Description.Should().Be(projectDescription);
        project.ProjectOwner.Should().Be(projectOwner);
        project.StartDate.Should().Be(DateOnly.FromDateTime(DateTime.Now));
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsNullOrEmpty))]
    [Trait("Domain", "Project - Aggregates")]
    [Trait("Domain", "Project - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    public void InstantiateErrorWhenNameIsNullOrEmpty(string projectName)
    {
        Action action =
            () => new Entity.Project(projectName, _fixture.GetValidProjectDescription(), _fixture.GetProjectOwner(), DateOnly.FromDateTime(DateTime.Now));

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Field Name cannot be null or empty");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenNameHasLessThanMinimumCharacteres))]
    [Trait("Domain", "Project - Aggregates")]
    [Trait("Domain", "Project - Aggregates")]
    public void InstantiateErrorWhenNameHasLessThanMinimumCharacteres()
    {
        var invalidProjectName = _fixture.GetValidProjectName()[..(ValidationConstants.Project_MinNameLength - 1)];

        Action action =
            () => new Entity.Project(invalidProjectName, _fixture.GetValidProjectDescription(), _fixture.GetProjectOwner(), DateOnly.FromDateTime(DateTime.Now));

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"Name should be at least {ValidationConstants.Project_MinNameLength} characters long");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenNameHasMoreThanMaximumCharacteres))]
    [Trait("Domain", "Project - Aggregates")]
    [Trait("Domain", "Project - Aggregates")]
    public void InstantiateErrorWhenNameHasMoreThanMaximumCharacteres()
    {
        var invalidProjectName = string.Empty;

        while (invalidProjectName.Length <= ValidationConstants.Project_MaxNameLength)
        {
            invalidProjectName += _fixture.GetValidProjectName();
        }

        Action action =
            () => new Entity.Project(invalidProjectName, _fixture.GetValidProjectDescription(), _fixture.GetProjectOwner(), DateOnly.FromDateTime(DateTime.Now));

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"Name should be less or equal {ValidationConstants.Project_MaxNameLength} characters long");
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNullOrEmpty))]
    [Trait("Domain", "Project - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    public void InstantiateErrorWhenDescriptionIsNullOrEmpty(string projectDescription)
    {
        Action action =
            () => new Entity.Project(_fixture.GetValidProjectName(), projectDescription, _fixture.GetProjectOwner(), DateOnly.FromDateTime(DateTime.Now));

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Field Description cannot be null or empty");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionHasLessThanMinimumCharacteres))]
    [Trait("Domain", "Project - Aggregates")]
    public void InstantiateErrorWhenDescriptionHasLessThanMinimumCharacteres()
    {
        var invalidProjectDescription = _fixture.GetValidProjectDescription()[..(ValidationConstants.Project_MinNameLength - 1)];

        Action action =
            () => new Entity.Project(_fixture.GetValidProjectName(), invalidProjectDescription, _fixture.GetProjectOwner(), DateOnly.FromDateTime(DateTime.Now));

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"Description should be at least {ValidationConstants.Project_MinDescriptionLength} characters long");
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenStartDateIsBeforeThanToday))]
    [Trait("Domain", "Project - Aggregates")]
    [MemberData(nameof(GetDatesBeforeToday), parameters: 10)]
    public void InstantiateErrorWhenStartDateIsBeforeThanToday(DateOnly startDate)
    {
        Action action =
            () => new Entity.Project(_fixture.GetValidProjectName(), _fixture.GetValidProjectDescription(), _fixture.GetProjectOwner(), startDate);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"StartDate must be the current date or later");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenProjectOwnerIsNull))]
    [Trait("Domain", "Project - Aggregates")]
    public void InstantiateErrorWhenProjectOwnerIsNull()
    {
        Action action =
            () => new Entity.Project(_fixture.GetValidProjectName(), _fixture.GetValidProjectDescription(), null!, DateOnly.FromDateTime(DateTime.Now));

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Field ProjectOwner cannot be null");
    }

    [Fact(DisplayName = nameof(SetProjectOwner))]
    [Trait("Domain", "Project - Aggregates")]
    public void SetProjectOwner()
    {
        var project = _fixture.GetProject();
        var projectOwner = _fixture.GetProjectOwner();

        project.SetProjectOwner(projectOwner);

        project.Should().NotBeNull();
        project.ProjectOwner.Should().NotBeNull();
        project.ProjectOwner.Should().Be(projectOwner);
    }

    [Fact(DisplayName = nameof(SetProjectOwnerErrorWhenProjectOwnerIsNull))]
    [Trait("Domain", "Project - Aggregates")]
    public void SetProjectOwnerErrorWhenProjectOwnerIsNull()
    {
        var project = _fixture.GetProject();
        var projectOwner = _fixture.GetProjectOwner();

        Action action =
               () => project.SetProjectOwner(null!);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Field ProjectOwner cannot be null");
    }

    [Fact(DisplayName = nameof(SetProjectOwnerErrorWhenProjectIsCompleted))]
    [Trait("Domain", "Project - Aggregates")]
    public void SetProjectOwnerErrorWhenProjectIsCompleted()
    {
        var project = _fixture.GetCompletedProject();
        var projectOwner = _fixture.GetProjectOwner();

        Action action =
               () => project.SetProjectOwner(projectOwner);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Completed project cannot to be changed");
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "Project - Aggregates")]
    public void Update()
    {
        var project = _fixture.GetProject();
        var newProjectName = _fixture.GetValidProjectName();
        var newProjectDescription = _fixture.GetValidProjectDescription();

        project.Update(newProjectName, newProjectDescription);

        project.Should().NotBeNull();
        project.Name.Should().Be(newProjectName);
        project.Description.Should().Be(newProjectDescription);
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsNullOrEmpty))]
    [Trait("Domain", "Project - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    public void UpdateErrorWhenNameIsNullOrEmpty(string projectName)
    {
        var project = _fixture.GetProject();
        var newProjectDescription = _fixture.GetValidProjectDescription();

        Action action =
            () => project.Update(projectName, newProjectDescription);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Field Name cannot be null or empty");
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenDescriptionIsNullOrEmpty))]
    [Trait("Domain", "Project - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    public void UpdateErrorWhenDescriptionIsNullOrEmpty(string projectDescription)
    {
        var project = _fixture.GetProject();
        var newProjectName = _fixture.GetValidProjectName();

        Action action =
            () => project.Update(newProjectName, projectDescription);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Field Description cannot be null or empty");
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenProjectIsCompleted))]
    [Trait("Domain", "Project - Aggregates")]
    public void UpdateErrorWhenProjectIsCompleted()
    {
        var project = _fixture.GetCompletedProject();
        var newProjectName = _fixture.GetValidProjectName();
        var newProjectDescription = _fixture.GetValidProjectDescription();

        Action action =
            () => project.Update(newProjectName, newProjectDescription);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Completed project cannot to be changed");
    }

    [Fact(DisplayName = nameof(AddTask))]
    [Trait("Domain", "Project - Aggregates")]
    public void AddTask()
    {
        var project = _fixture.GetProject();
        var taskTitle = _fixture.GetValidTaskTitle();
        var taskDescription = _fixture.GetValidTaskDescription();

        project.AddTask(taskTitle, taskDescription);

        project.Should().NotBeNull();
        project.Tasks.Should().NotBeEmpty();
        project.Tasks.First().Title.Should().Be(taskTitle);
        project.Tasks.First().Description.Should().Be(taskDescription);
    }

    [Theory(DisplayName = nameof(AddTaskErrorWhenTitleIsNullOrEmpty))]
    [Trait("Domain", "Project - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    public void AddTaskErrorWhenTitleIsNullOrEmpty(string taskTitle)
    {
        var project = _fixture.GetProject();
        var taskDescription = _fixture.GetValidTaskDescription();

        Action action =
               () => project.AddTask(taskTitle, taskDescription);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Field Title cannot be null or empty");
    }

    [Theory(DisplayName = nameof(AddTaskErrorWhenDescriptionIsNullOrEmpty))]
    [Trait("Domain", "Project - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    public void AddTaskErrorWhenDescriptionIsNullOrEmpty(string taskDescription)
    {
        var project = _fixture.GetProject();
        var taskTitle = _fixture.GetValidTaskTitle();

        Action action =
               () => project.AddTask(taskTitle, taskDescription);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Field Description cannot be null or empty");
    }

    [Fact(DisplayName = nameof(AddTaskErrorWhenTitleHasLessThanMinimumCharacteres))]
    [Trait("Domain", "Project - Aggregates")]
    public void AddTaskErrorWhenTitleHasLessThanMinimumCharacteres()
    {
        var project = _fixture.GetProject();
        var invalidTaskTitle = _fixture.GetValidProjectName()[..(ValidationConstants.Task_MinTitleLength - 1)];
        var taskDescription = _fixture.GetValidTaskDescription();

        Action action =
            () => project.AddTask(invalidTaskTitle, taskDescription);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"Title should be at least {ValidationConstants.Task_MinTitleLength} characters long");
    }

    [Fact(DisplayName = nameof(AddTaskErrorWhenTitleHasMoreThanMaximumCharacteres))]
    [Trait("Domain", "Project - Aggregates")]
    public void AddTaskErrorWhenTitleHasMoreThanMaximumCharacteres()
    {
        var project = _fixture.GetProject();
        var invalidTaskTitle = string.Empty;
        var taskDescription = _fixture.GetValidTaskTitle();
        while (invalidTaskTitle.Length <= ValidationConstants.Task_MaxTitleLength)
        {
            invalidTaskTitle += _fixture.GetValidTaskTitle();
        }

        Action action =
            () => project.AddTask(invalidTaskTitle, taskDescription);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"Title should be less or equal {ValidationConstants.Task_MaxTitleLength} characters long");
    }

    [Fact(DisplayName = nameof(AddTaskErrorWhenDescriptionHasLessThanMinimumCharacteres))]
    [Trait("Domain", "Project - Aggregates")]
    public void AddTaskErrorWhenDescriptionHasLessThanMinimumCharacteres()
    {
        var project = _fixture.GetProject();
        var title = _fixture.GetValidTaskTitle();
        var invalidDaskDescription = _fixture.GetValidTaskDescription()[..(ValidationConstants.Task_MinDescriptionLength - 1)];

        Action action =
            () => project.AddTask(title, invalidDaskDescription);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"Description should be at least {ValidationConstants.Task_MinDescriptionLength} characters long");
    }

    [Fact(DisplayName = nameof(RemoveTask))]
    [Trait("Domain", "Project - Aggregates")]
    public void RemoveTask()
    {
        var project = _fixture.GetProject();
        project.AddTask(_fixture.GetValidTaskTitle(), _fixture.GetValidTaskDescription());
        project.AddTask(_fixture.GetValidTaskTitle(), _fixture.GetValidTaskDescription());
        project.AddTask(_fixture.GetValidTaskTitle(), _fixture.GetValidTaskDescription());
        var taskToBeRemoved = project.Tasks.Last();

        project.RemoveTask(taskToBeRemoved);

        project.Should().NotBeNull();
        project.Tasks.Should().NotBeEmpty();
        project.Tasks.Should().NotContain(taskToBeRemoved);
        project.Tasks.Count.Should().Be(2);
    }

    [Fact(DisplayName = nameof(RemoveTaskErrorWhenTaskIsNull))]
    [Trait("Domain", "Project - Aggregates")]
    public void RemoveTaskErrorWhenTaskIsNull()
    {
        var project = _fixture.GetProject();

        Action action =
            () => project.RemoveTask(null!);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"Field task cannot be null");
    }

    [Fact(DisplayName = nameof(RemoveTaskErrorWhenTaskDoesNotExists))]
    [Trait("Domain", "Project - Aggregates")]
    public void RemoveTaskErrorWhenTaskDoesNotExists()
    {
        var project = _fixture.GetProject();
        var task = _fixture.GetTask();

        Action action =
            () => project.RemoveTask(task);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"Task {task.Title} does not exists in this project");
    }

    [Fact(DisplayName = nameof(AddProjectMember))]
    [Trait("Domain", "Project - Aggregates")]
    public void AddProjectMember()
    {
        var project = _fixture.GetProject();
        var projectMember = _fixture.GetProjectMember();

        project.AddProjectMember(projectMember);

        project.Should().NotBeNull();
        project.ProjectMembers.Should().NotBeEmpty();
        project.ProjectMembers.Should().Contain(projectMember);
    }

    [Fact(DisplayName = nameof(AddProjectMemberErrorWhenProjectMemberIsNull))]
    [Trait("Domain", "Project - Aggregates")]
    public void AddProjectMemberErrorWhenProjectMemberIsNull()
    {
        var project = _fixture.GetProject();

        Action action =
            () => project.AddProjectMember(null!);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"Field ProjectMember cannot be null");
    }

    [Fact(DisplayName = nameof(AddProjectMemberErrorWhenProjectMemberAlreadyExistsInProject))]
    [Trait("Domain", "Project - Aggregates")]
    public void AddProjectMemberErrorWhenProjectMemberAlreadyExistsInProject()
    {
        var project = _fixture.GetProject();
        var projectMember = _fixture.GetProjectMember();
        project.AddProjectMember(projectMember);

        Action action =
            () => project.AddProjectMember(projectMember);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"ProjectMember {projectMember.Name} is already member of this project");
    }

    [Fact(DisplayName = nameof(AddProjectMemberErrorWhenProjectHasMaxProjectMembersAlready))]
    [Trait("Domain", "Project - Aggregates")]
    public void AddProjectMemberErrorWhenProjectHasMaxProjectMembersAlready()
    {
        var project = _fixture.GetProject();
        var projectMember = _fixture.GetProjectMember();
        project.AddProjectMember(projectMember);
        project.AddTask(_fixture.GetValidTaskTitle(), _fixture.GetValidTaskDescription());
        var task = project.Tasks[0];
        task.SetProjectMember(projectMember);

        Action action =
            () => project.RemoveProjectMember(projectMember);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"The ProjectMember {projectMember.Name} cannot be removed because has one or more related tasks");
    }

    [Fact(DisplayName = nameof(RemoveProjectMember))]
    [Trait("Domain", "Project - Aggregates")]
    public void RemoveProjectMember()
    {
        var project = _fixture.GetProject();
        var projectMember = _fixture.GetProjectMember();
        project.AddProjectMember(projectMember);

        project.RemoveProjectMember(projectMember);

        project.Should().NotBeNull();
        project.ProjectMembers.Should().BeEmpty();
        project.ProjectMembers.Should().NotContain(projectMember);
    }

    [Fact(DisplayName = nameof(RemoveProjectMemberErrorWhenProjectMemberIsNull))]
    [Trait("Domain", "Project - Aggregates")]
    public void RemoveProjectMemberErrorWhenProjectMemberIsNull()
    {
        var project = _fixture.GetProject();

        Action action =
            () => project.RemoveProjectMember(null!);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"Field ProjectMember cannot be null");
    }

    [Fact(DisplayName = nameof(RemoveProjectMemberErrorWhenProjectMemberDoesNotExistsInProject))]
    [Trait("Domain", "Project - Aggregates")]
    public void RemoveProjectMemberErrorWhenProjectMemberDoesNotExistsInProject()
    {
        var project = _fixture.GetProject();
        project.AddProjectMember(_fixture.GetProjectMember());
        project.AddProjectMember(_fixture.GetProjectMember());
        var projectMember = _fixture.GetProjectMember();

        Action action =
            () => project.RemoveProjectMember(projectMember);

        action.Should()
        .Throw<EntityValidationException>()
            .WithMessage($"ProjectMember {projectMember.Name} does not is member of this project");
    }

    [Fact(DisplayName = nameof(RemoveProjectMemberErrorWhenProjectMemberHasOneOrMoreTaksRelated))]
    [Trait("Domain", "Project - Aggregates")]
    public void RemoveProjectMemberErrorWhenProjectMemberHasOneOrMoreTaksRelated()
    {
        var project = _fixture.GetProject();
        Enumerable.Range(0, ValidationConstants.Project_MaxProjectMembers).ToList().ForEach(x =>
        {
            project.AddProjectMember(_fixture.GetProjectMember());
        });

        Action action =
            () => project.AddProjectMember(_fixture.GetProjectMember());

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"Project {project.Name} has already reached the maximum number of members");
    }

    [Fact(DisplayName = nameof(ChangeStatusCreatedToInProgess))]
    [Trait("Domain", "Project - Aggregates")]
    public void ChangeStatusCreatedToInProgess()
    {
        var project = _fixture.GetProject();

        project.ChangeStatus(ProjectStatus.InProgress);

        project.Status.Should().Be(ProjectStatus.InProgress);
    }

    [Fact(DisplayName = nameof(ChangeStatusInProgressToCompleted))]
    [Trait("Domain", "Project - Aggregates")]
    public void ChangeStatusInProgressToCompleted()
    {
        var project = _fixture.GetProject();
        project.ChangeStatus(ProjectStatus.InProgress);

        project.ChangeStatus(ProjectStatus.Completed);

        project.Status.Should().Be(ProjectStatus.Completed);
    }

    [Fact(DisplayName = nameof(ChangeStatusCreateToCanceled))]
    [Trait("Domain", "Project - Aggregates")]
    public void ChangeStatusCreateToCanceled()
    {
        var project = _fixture.GetProject();

        project.ChangeStatus(ProjectStatus.Canceled);

        project.Status.Should().Be(ProjectStatus.Canceled);
    }

    [Fact(DisplayName = nameof(ChangeStatusInProgressToCanceled))]
    [Trait("Domain", "Project - Aggregates")]
    public void ChangeStatusInProgressToCanceled()
    {
        var project = _fixture.GetProject();
        project.ChangeStatus(ProjectStatus.InProgress);

        project.ChangeStatus(ProjectStatus.Canceled);

        project.Status.Should().Be(ProjectStatus.Canceled);
    }

    [Fact(DisplayName = nameof(ChangeStatusErrorWhenTransitionIsInvalid))]
    [Trait("Domain", "Project - Aggregates")]
    public void ChangeStatusErrorWhenTransitionIsInvalid()
    {
        var project = _fixture.GetProject();
        var newStatus = ProjectStatus.Completed;

        Action action =
            () => project.ChangeStatus(ProjectStatus.Completed);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"Invalid project status transition from {project.Status} to {newStatus}");
    }

    [Fact(DisplayName = nameof(SetDeadline))]
    [Trait("Domain", "Project - Aggregates")]
    public void SetDeadline()
    {
        var project = _fixture.GetProject();
        var deadLine = project.StartDate.AddDays(1);

        project.SetDeadline(deadLine);

        project.Deadline.Should().Be(deadLine);
    }

    [Fact(DisplayName = nameof(SetDeadlineErrorWhenDeadlineIsBeforeThanToday))]
    [Trait("Domain", "Project - Aggregates")]
    public void SetDeadlineErrorWhenDeadlineIsBeforeThanToday()
    {
        var project = _fixture.GetProject();
        var deadLine = DateOnly.FromDateTime(DateTime.Now.AddDays(-1));

        Action action =
                () => project.SetDeadline(deadLine);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"Deadline must be the current date or later");
    }

    [Fact(DisplayName = nameof(SetDeadlineErrorWhenDeadlineIsBeforeStartDate))]
    [Trait("Domain", "Project - Aggregates")]
    public void SetDeadlineErrorWhenDeadlineIsBeforeStartDate()
    {
        var project = _fixture.GetProject(DateTime.Now.AddDays(1));
        var deadLine = project.StartDate.AddDays(-1);

        Action action =
                () => project.SetDeadline(deadLine);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"Project deadline must be after the start date");
    }

    public static IEnumerable<object[]> GetDatesBeforeToday(int days)
    {
        var datesBeforeToday = Enumerable.Range(0, days).Select(d =>
        {
            var daysBefore = new Random().Next(1, 10) * -1;

            return new object[] { DateOnly.FromDateTime(DateTime.Now.AddDays(daysBefore)) };
        });

        return datesBeforeToday;
    }
}

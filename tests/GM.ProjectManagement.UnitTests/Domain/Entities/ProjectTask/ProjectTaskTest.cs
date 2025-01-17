using FluentAssertions;
using GM.ProjectManagement.Domain.Enums;
using GM.ProjectManagement.Domain.Exceptions;

namespace GM.ProjectManagement.UnitTests.Domain.Entities.ProjectTask;

[Collection(nameof(ProjectTaskTestFixture))]
public class ProjectTaskTest(ProjectTaskTestFixture fixture)
{
    private readonly ProjectTaskTestFixture _fixture = fixture;

    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "ProjectTask - Aggregates")]
    public void Update()
    {
        var task = _fixture.GetTask();
        var newTaskTitle = _fixture.GetValidTaskTitle();
        var newTaskDescription = _fixture.GetValidTaskDescription();

        task.Update(newTaskTitle, newTaskDescription);

        task.Should().NotBeNull();
        task.Title.Should().Be(newTaskTitle);
        task.Description.Should().Be(newTaskDescription);
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenTitleIsNullOrEmpty))]
    [Trait("Domain", "ProjectTask - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    public void UpdateErrorWhenTitleIsNullOrEmpty(string taskTitle)
    {
        var task = _fixture.GetTask();
        var newTaskDescription = _fixture.GetValidTaskDescription();

        Action action =
            () => task.Update(taskTitle, newTaskDescription);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Field Title cannot be null or empty");
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenDescriptionIsNullOrEmpty))]
    [Trait("Domain", "ProjectTask - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    public void UpdateErrorWhenDescriptionIsNullOrEmpty(string taskDescription)
    {
        var task = _fixture.GetTask();
        var newTaskTitle = _fixture.GetValidTaskTitle();

        Action action =
            () => task.Update(newTaskTitle, taskDescription);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Field Description cannot be null or empty");
    }

    [Fact(DisplayName = nameof(SetDueDate))]
    [Trait("Domain", "ProjectTask - Aggregates")]
    public void SetDueDate()
    {
        var task = _fixture.GetTask();
        var dueDate = DateOnly.FromDateTime(DateTime.Now);

        task.SetDueDate(dueDate);

        task.DueDate.Should().NotBeNull();
        task.DueDate.Should().Be(dueDate);
    }

    [Theory(DisplayName = nameof(SetDueDateErrorWhenDueDateIsBeforeThanToday))]
    [Trait("Domain", "ProjectTask - Aggregates")]
    [MemberData(nameof(GetDatesBeforeToday), parameters: 10)]
    public void SetDueDateErrorWhenDueDateIsBeforeThanToday(DateOnly dueDate)
    {
        var task = _fixture.GetTask();

        Action action =
             () => task.SetDueDate(dueDate);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"DueDate must be the current date or later");
    }

    [Fact(DisplayName = nameof(SetProjectMember))]
    [Trait("Domain", "ProjectTask - Aggregates")]
    public void SetProjectMember()
    {
        var task = _fixture.GetTask();
        var projectMember = _fixture.GetProjectMember();

        task.SetProjectMember(projectMember);

        task.ProjectMember.Should().NotBeNull();
        task.ProjectMember.Should().Be(projectMember);
        task.ProjectMemberId.Should().Be(projectMember.Id);
    }

    [Fact(DisplayName = nameof(SetProjectMemberNull))]
    [Trait("Domain", "ProjectTask - Aggregates")]
    public void SetProjectMemberNull()
    {
        var task = _fixture.GetTask();
        var projectMember = _fixture.GetProjectMember();
        task.SetProjectMember(projectMember);

        task.SetProjectMember(null);

        task.ProjectMember.Should().BeNull();
    }

    [Fact(DisplayName = nameof(ChangeStatusBacklogToApproved))]
    [Trait("Domain", "ProjectTask - Aggregates")]
    public void ChangeStatusBacklogToApproved()
    {
        var task = _fixture.GetTask();

        task.ChangeStatus(ProjectTaskStatus.Approved);

        task.Status.Should().Be(ProjectTaskStatus.Approved);
    }


    [Fact(DisplayName = nameof(ChangeStatusApprovedToInProgess))]
    [Trait("Domain", "ProjectTask - Aggregates")]
    public void ChangeStatusApprovedToInProgess()
    {
        var task = _fixture.GetTask();
        task.ChangeStatus(ProjectTaskStatus.Approved);

        task.ChangeStatus(ProjectTaskStatus.InProgress);

        task.Status.Should().Be(ProjectTaskStatus.InProgress);
    }

    [Fact(DisplayName = nameof(ChangeStatusInProgessToValidation))]
    [Trait("Domain", "ProjectTask - Aggregates")]
    public void ChangeStatusInProgessToValidation()
    {
        var task = _fixture.GetTask();
        task.ChangeStatus(ProjectTaskStatus.Approved);
        task.ChangeStatus(ProjectTaskStatus.InProgress);

        task.ChangeStatus(ProjectTaskStatus.Validation);

        task.Status.Should().Be(ProjectTaskStatus.Validation);
    }

    [Fact(DisplayName = nameof(ChangeStatusValidationToCompleted))]
    [Trait("Domain", "ProjectTask - Aggregates")]
    public void ChangeStatusValidationToCompleted()
    {
        var task = _fixture.GetTask();
        task.ChangeStatus(ProjectTaskStatus.Approved);
        task.ChangeStatus(ProjectTaskStatus.InProgress);
        task.ChangeStatus(ProjectTaskStatus.Validation);

        task.ChangeStatus(ProjectTaskStatus.Completed);

        task.Status.Should().Be(ProjectTaskStatus.Completed);
    }

    [Fact(DisplayName = nameof(ChangeStatusErrorWhenTransitionIsInvalid))]
    [Trait("Domain", "ProjectTask - Aggregates")]
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

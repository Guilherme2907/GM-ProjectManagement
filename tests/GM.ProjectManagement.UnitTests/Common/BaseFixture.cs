using Bogus;
using GM.ProjectManagement.Domain.Entities;
using GM.ProjectManagement.Domain.Entities.ValueObjects;
using GM.ProjectManagement.Domain.Enums;
using GM.ProjectManagement.Domain.Validations;

namespace GM.ProjectManagement.UnitTests.Common;

public abstract class BaseFixture
{
    protected Faker _faker;

    public BaseFixture()
     => _faker = new Faker("pt_BR");


    public ProjectMember GetProjectMember(bool isAdmin = false)
    {
        return new
        (
            isAdmin ? ProjectMemberRole.Admin : GetRandomStatusValue<ProjectMemberRole>(),
            Guid.NewGuid()
        );
    }

    public T GetRandomStatusValue<T>() where T : Enum
    {
        var values = Enum.GetValues(typeof(T));
        var randomIndex = new Random().Next(values.Length);
        return (T)values.GetValue(randomIndex)!;
    }

    public ProjectOwner GetProjectOwner()
    {
        return new(GetProjectMember(isAdmin: true));
    }

    public Project GetProject(DateTime? startDate = null)
    {
        startDate = startDate ?? DateTime.Now;

        return new
        (
            GetValidProjectName(),
            GetValidProjectDescription(),
            GetProjectOwner(),
            DateOnly.FromDateTime(startDate.Value)
        );
    }

    public Project GetCompletedProject()
    {
        var project = GetProject();

        project.ChangeStatus(ProjectStatus.InProgress);
        project.ChangeStatus(ProjectStatus.Completed);

        return project;
    }

    public ProjectTask GetTask()
    {
        var project = GetProject();

        project.AddTask(GetValidTaskTitle(), GetValidTaskDescription());

        return project.Tasks[0];
    }

    public string GetValidEmail()
    {
        return _faker.Person.Email;
    }

    public string GetValidPassword()
    {
        return _faker.Person.Email;
    }

    public string GetValidProjectMemberName()
    {
        var name = _faker.Person.FirstName;

        while (name.Length < ValidationConstants.ProjectMember_MinNameLength)
            name += _faker.Person.FirstName;

        name = name.Length > ValidationConstants.ProjectMember_MaxNameLength ?
            name[..ValidationConstants.ProjectMember_MaxNameLength]
            : name;

        return name;
    }

    public string GetValidProjectMemberSurname()
    {
        var name = _faker.Person.LastName;

        while (name.Length < ValidationConstants.ProjectMember_MinSurNameLength)
            name += _faker.Person.LastName;

        name = name.Length > ValidationConstants.ProjectMember_MaxNameLength ?
            name[..ValidationConstants.ProjectMember_MaxNameLength]
            : name;

        return name;
    }

    public string GetValidProjectName()
    {
        var name = _faker.Commerce.ProductName();

        while (name.Length < ValidationConstants.Project_MinNameLength)
            name += _faker.Commerce.ProductName();

        name = name.Length > ValidationConstants.Project_MaxNameLength ?
            name[..ValidationConstants.Project_MaxNameLength]
            : name;

        return name;
    }

    public string GetValidProjectDescription()
    {
        var description = _faker.Commerce.ProductDescription();

        while (description.Length < ValidationConstants.Project_MinDescriptionLength)
            description += _faker.Commerce.ProductDescription();

        return description;
    }

    public string GetValidTaskTitle()
    {
        var title = _faker.Commerce.ProductName();

        while (title.Length < ValidationConstants.Task_MinTitleLength)
            title += _faker.Commerce.ProductName();

        title = title.Length > ValidationConstants.Task_MaxTitleLength ?
            title[..ValidationConstants.Task_MaxTitleLength]
            : title;

        return title;
    }

    public string GetValidTaskDescription()
    {
        var description = _faker.Commerce.ProductName();

        while (description.Length < ValidationConstants.Task_MinDescriptionLength)
            description += _faker.Commerce.ProductName();

        return description;
    }
}

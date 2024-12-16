using GM.ProjectManagement.Domain.Enums;
using GM.ProjectManagement.Domain.SeedWork;

namespace GM.ProjectManagement.Domain.Entities;

public class User : Entity
{
    public string Username { get; private set; }
    public string Email { get; private set; }
    public string FullName { get; private set; }
    public UserRole Role { get; private set; }
    public bool IsActive { get; private set; } = true;

    public ICollection<Project> Projects { get; set; } = [];
    public ICollection<ProjectTask> Tasks { get; set; } = [];

    public User
    (
        string username,
        string email,
        string fullName,
        UserRole role
    )
    {
        Username = username;
        Email = email;
        FullName = fullName;
        Role = role;
    }

    private void Activate()
    {
        IsActive = true;
    }

    private void Deactivate()
    {
        IsActive = false;
    }

    private void AddProject(Project project)
    {
        Projects.Add(project);
    }   
    
    private void AddTask(ProjectTask task)
    {
        Tasks.Add(task);    
    }
}

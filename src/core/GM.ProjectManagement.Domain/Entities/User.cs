using GM.ProjectManagement.Domain.SeedWork;

namespace GM.ProjectManagement.Domain.Entities;

public class User : Entity
{
    public string Email { get; private set; }

    public string Password { get; private set; }

    public bool IsActive { get; private set; } = true;

    public Guid? ProjectMemberId { get; private set; }

    public User(string email, string password)
    {
        Email = email;
        Password = password;
    }

    public void UpdatePassword(string password)
    {
        Password = password;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}

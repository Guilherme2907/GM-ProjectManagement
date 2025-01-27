using GM.ProjectManagement.Application.UseCases.CreateUser;

namespace GM.ProjectManagement.Infrastructure.Auth.Keycloack.KeycloakModels.CreateUser;

public class KeycloackCreateUserRequest
{
    public string Username { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public bool Enabled { get; set; }

    public IList<KeycloackCredential> Credentials { get; set; } = [];

    public KeycloackCreateUserRequest(
        string username,
        string firtName,
        string lastName,
        string email,
        bool enabled,
        KeycloackCredential credential
    )
    {
        Username = username;
        FirstName = firtName;
        LastName = lastName;
        Email = email;
        Enabled = enabled;
        Credentials.Add(credential);
    }

    public static KeycloackCreateUserRequest FromCreateUserInput(CreateUserInput input)
    {
        var credential = new KeycloackCredential("password", input.Password);

        return new KeycloackCreateUserRequest(
            input.Username,
            input.FirstName,
            input.LastName,
            input.Email,
            input.Enabled,
            credential
        );
    }
}

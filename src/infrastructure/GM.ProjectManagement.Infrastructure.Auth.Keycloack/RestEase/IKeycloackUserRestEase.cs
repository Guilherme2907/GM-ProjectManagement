using GM.ProjectManagement.Infrastructure.Auth.Keycloack.KeycloakModels.CreateUser;
using RestEase;

namespace GM.ProjectManagement.Infrastructure.Auth.Keycloack.RestEase;

public interface IKeycloackUserRestEase
{
    [Header("Authorization")]
    string AuthorizationHeader { get; set; }

    [Post("/admin/realms/ProjectManagement/users")]
    Task CreateUserAsync([Body] KeycloackCreateUserRequest keycloackLoginUserRequest, CancellationToken cancellation);
}

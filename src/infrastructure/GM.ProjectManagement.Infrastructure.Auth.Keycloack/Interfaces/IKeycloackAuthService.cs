using GM.ProjectManagement.Application.Interfaces;
using GM.ProjectManagement.Infrastructure.Auth.Keycloack.KeycloakModels.Login;

namespace GM.ProjectManagement.Infrastructure.Auth.Keycloack.Interfaces;

public interface IKeycloackAuthService : IAuthService
{
    Task<KeycloackLoginResponse?> GetAdminTokenAsync(CancellationToken cancellation);
}

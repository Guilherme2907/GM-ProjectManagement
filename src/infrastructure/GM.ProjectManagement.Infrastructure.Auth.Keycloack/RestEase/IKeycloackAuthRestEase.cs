using GM.ProjectManagement.Infrastructure.Auth.Keycloack.KeycloakModels.Login;
using RestEase;

namespace GM.ProjectManagement.Infrastructure.Auth.Keycloack.RestEase;

public interface IKeycloackAuthRestEase
{
    [Post("/realms/ProjectManagement/protocol/openid-connect/token")]
    Task<KeycloackLoginResponse> LoginKeycloakAsync([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, string> data, CancellationToken cancellation);
}

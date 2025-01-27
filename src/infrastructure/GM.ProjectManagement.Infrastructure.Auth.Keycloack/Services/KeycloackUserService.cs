using GM.ProjectManagement.Application.Interfaces;
using GM.ProjectManagement.Application.UseCases.CreateUser;
using GM.ProjectManagement.Infrastructure.Auth.Keycloack.Interfaces;
using GM.ProjectManagement.Infrastructure.Auth.Keycloack.KeycloakModels.CreateUser;
using GM.ProjectManagement.Infrastructure.Auth.Keycloack.RestEase;

namespace GM.ProjectManagement.Infrastructure.Auth.Keycloack.Services;

public class KeycloackUserService(
        IKeycloackAuthService keycloackAuthService,
        IKeycloackUserRestEase keycloackUserRestEase
    ) : IUserService
{
    private readonly IKeycloackAuthService _keycloackAuthService = keycloackAuthService;
    private readonly IKeycloackUserRestEase _keycloackuserRestEase = keycloackUserRestEase;

    public async Task CreateAsync(CreateUserInput input, CancellationToken cancellationToken)
    {
        var admLoginResponse = await _keycloackAuthService.GetAdminTokenAsync(cancellationToken) ?? throw new Exception("Token null");

        var request = KeycloackCreateUserRequest.FromCreateUserInput(input);

        _keycloackuserRestEase.AuthorizationHeader = $"{admLoginResponse.TokenType} {admLoginResponse!.AccessToken}";
        await _keycloackuserRestEase.CreateUserAsync(request, cancellationToken);
    }
}

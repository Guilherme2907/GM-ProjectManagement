using GM.ProjectManagement.Application.UseCases.LoginUser;
using GM.ProjectManagement.Infrastructure.Auth.Keycloack.Interfaces;
using GM.ProjectManagement.Infrastructure.Auth.Keycloack.KeycloakModels.Login;
using GM.ProjectManagement.Infrastructure.Auth.Keycloack.RestEase;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace GM.ProjectManagement.Infrastructure.Auth.Keycloack.Services;

public class KeycloackAuthService(
        IConfiguration configuration,
        IMemoryCache memoryCache,
        IKeycloackAuthRestEase keycloackAuthRestEase
    ) : IKeycloackAuthService
{
    private readonly string? _clientId = configuration["Keycloak:ClientId"];
    private readonly string? _clientSecret = configuration["Keycloak:ClientSecret"];
    private readonly IMemoryCache _memoryCache = memoryCache;
    private readonly IKeycloackAuthRestEase _keycloackAuthRestEase = keycloackAuthRestEase;

    public async Task<KeycloackLoginResponse?> GetAdminTokenAsync(CancellationToken cancellationToken)
    {
        return await _memoryCache.GetOrCreateAsync(KeycloackConstants.Memory_Cache_Key, async entry =>
        {
            var adminLoginRequest = new Dictionary<string, string> {
                {KeycloackConstants.Client_Id, _clientId!},
                {KeycloackConstants.Client_Secret, _clientSecret!},
                {KeycloackConstants.Grant_Type, KeycloackConstants.Grant_Type_Credentials}
            };

            var response = await _keycloackAuthRestEase.LoginKeycloakAsync(adminLoginRequest, cancellationToken);

            entry.SetSlidingExpiration(TimeSpan.FromSeconds(300));

            return response;
        });
    }

    public async Task<LoginUserOutput> LoginUserAsync(LoginUserInput input, CancellationToken cancellationToken)
    {
        var userLoginRequest = new Dictionary<string, string> {
                {KeycloackConstants.Client_Id, _clientId!},
                {KeycloackConstants.Client_Secret, _clientSecret!},
                {KeycloackConstants.Grant_Type, KeycloackConstants.Grant_Type_Password},
                {KeycloackConstants.Username, input.Username},
                {KeycloackConstants.Password, input.Password}
            };


        var response = await _keycloackAuthRestEase.LoginKeycloakAsync(userLoginRequest, cancellationToken);

        return new(
                response.AccessToken,
                response.ExpiresIn,
                response.RefreshExpiresIn,
                response.RefreshToken,
                response.TokenType
            );
    }
}

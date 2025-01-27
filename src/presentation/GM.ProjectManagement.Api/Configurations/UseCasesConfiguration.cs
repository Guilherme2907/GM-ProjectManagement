using GM.ProjectManagement.Application.Interfaces;
using GM.ProjectManagement.Application.UseCases.CreateUser;
using GM.ProjectManagement.Infrastructure.Auth.Keycloack.Interfaces;
using GM.ProjectManagement.Infrastructure.Auth.Keycloack.Services;

namespace GM.ProjectManagement.Api.Configurations;

public static class UseCasesConfiguration
{
    public static IServiceCollection AddUseCasesConfiguration(this IServiceCollection services)
    {
        services
            .AddMediatR(config => config.RegisterServicesFromAssemblies(typeof(CreateUser).Assembly))
            .AddMemoryCache()
            .AddScoped<IUserService, KeycloackUserService>()
            .AddScoped<IAuthService, KeycloackAuthService>()
            .AddScoped<IKeycloackAuthService, KeycloackAuthService>();

        return services;
    }
}

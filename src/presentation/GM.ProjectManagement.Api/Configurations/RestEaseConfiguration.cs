using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using RestEase;
using GM.ProjectManagement.Infrastructure.Auth.Keycloack.RestEase;

namespace GM.ProjectManagement.Api.Configurations;

public static class RestEaseConfiguration
{
    private static readonly JsonSerializerSettings _camelCaseJsonSerializerSettings = new()
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        Converters = { new StringEnumConverter() }
    };

    public static IServiceCollection AddReastEaseConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddSingleton(CreateRestClient(configuration["Keycloak:CreateUserBaseUrl"]!, _camelCaseJsonSerializerSettings).For<IKeycloackUserRestEase>())
            .AddSingleton(CreateRestClient(configuration["Keycloak:CreateUserBaseUrl"]!, _camelCaseJsonSerializerSettings).For<IKeycloackAuthRestEase>());

        return services;
    }

    private static RestClient CreateRestClient(string baseUrl, JsonSerializerSettings settings)
    {
        return new RestClient(baseUrl)
        {
            JsonSerializerSettings = settings
        };
    }
}

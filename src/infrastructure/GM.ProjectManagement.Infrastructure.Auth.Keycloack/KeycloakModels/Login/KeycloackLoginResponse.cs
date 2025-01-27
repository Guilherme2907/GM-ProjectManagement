﻿using Newtonsoft.Json;

namespace GM.ProjectManagement.Infrastructure.Auth.Keycloack.KeycloakModels.Login;

public record KeycloackLoginResponse
{
    [JsonProperty("access_token")]
    public string AccessToken { get; set; }

    [JsonProperty("expires_in")]
    public int ExpiresIn { get; set; }

    [JsonProperty("refresh_expires_in")]
    public int RefreshExpiresIn { get; set; }

    [JsonProperty("refresh_token")]
    public string RefreshToken { get; set; }

    [JsonProperty("token_type")]
    public string TokenType { get; set; }
}

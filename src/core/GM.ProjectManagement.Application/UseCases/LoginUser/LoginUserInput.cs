using MediatR;
using Newtonsoft.Json;

namespace GM.ProjectManagement.Application.UseCases.LoginUser;

public class LoginUserInput : IRequest<LoginUserOutput>
{
    [JsonProperty("user_name")]
    public string Username { get; set; }

    [JsonProperty("password")]
    public string Password { get; set; }

    public LoginUserInput(string username, string password)
    {
        Username = username;
        Password = password;
    }
}

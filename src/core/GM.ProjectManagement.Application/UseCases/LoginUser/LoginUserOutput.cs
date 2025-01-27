namespace GM.ProjectManagement.Application.UseCases.LoginUser;

public record LoginUserOutput(
    string AccessToken,
    int ExpiresIn,
    int RefreshExpiresIn,
    string RefreshToken,
    string TokenType
);

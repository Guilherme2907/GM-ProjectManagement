using GM.ProjectManagement.Application.UseCases.LoginUser;

namespace GM.ProjectManagement.Application.Interfaces;

public interface IAuthService
{
    Task<LoginUserOutput> LoginUserAsync(LoginUserInput input, CancellationToken cancellationToken);
}

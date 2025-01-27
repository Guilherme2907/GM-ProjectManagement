
using GM.ProjectManagement.Application.Interfaces;

namespace GM.ProjectManagement.Application.UseCases.LoginUser;

public class LoginUser(IAuthService authService) : ILoginUser
{
    private readonly IAuthService _authService = authService;

    public async Task<LoginUserOutput> Handle(LoginUserInput request, CancellationToken cancellationToken)
    {
        return await _authService.LoginUserAsync(request, cancellationToken);
    }
}

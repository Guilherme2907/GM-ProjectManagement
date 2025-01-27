using GM.ProjectManagement.Application.Interfaces;

namespace GM.ProjectManagement.Application.UseCases.CreateUser;

public class CreateUser(IUserService userService) : ICreateUser
{
    private readonly IUserService _userService = userService;

    public async Task Handle(CreateUserInput request, CancellationToken cancellationToken)
    {
        //Todo - Validate input before send to service

        await _userService.CreateAsync(request, cancellationToken);
    }
}

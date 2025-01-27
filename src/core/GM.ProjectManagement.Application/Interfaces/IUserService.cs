using GM.ProjectManagement.Application.UseCases.CreateUser;
using GM.ProjectManagement.Application.UseCases.LoginUser;

namespace GM.ProjectManagement.Application.Interfaces;

public interface IUserService
{
    Task CreateAsync(CreateUserInput input, CancellationToken cancellationToken);
}

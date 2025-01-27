using MediatR;

namespace GM.ProjectManagement.Application.UseCases.LoginUser;
public interface ILoginUser : IRequestHandler<LoginUserInput, LoginUserOutput>
{
}

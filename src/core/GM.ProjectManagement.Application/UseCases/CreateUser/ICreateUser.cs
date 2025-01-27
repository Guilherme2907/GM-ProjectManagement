using MediatR;

namespace GM.ProjectManagement.Application.UseCases.CreateUser;

public interface ICreateUser : IRequestHandler<CreateUserInput>
{
}

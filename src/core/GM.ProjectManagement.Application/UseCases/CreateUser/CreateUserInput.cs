using MediatR;

namespace GM.ProjectManagement.Application.UseCases.CreateUser;

public record CreateUserInput(
    string FirstName,
    string LastName,
    string Username,
    string Email,
    bool Enabled,
    string Password
) : IRequest;


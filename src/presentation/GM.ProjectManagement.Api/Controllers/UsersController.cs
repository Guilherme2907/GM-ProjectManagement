using GM.ProjectManagement.Application.UseCases.CreateUser;
using GM.ProjectManagement.Application.UseCases.LoginUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GM.ProjectManagement.Api.Controllers;

[ApiController]
[Route("users")]
public class UsersController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateUserInput input,CancellationToken cancellationToken)
    {
        await _mediator.Send(input, cancellationToken);

        return Created();
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] LoginUserInput input, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(input, cancellationToken);

        return Ok(response);
    }
}


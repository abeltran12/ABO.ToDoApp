using ABO.ToDoApp.Application.Feautures.Identity.Login;
using ABO.ToDoApp.Application.Feautures.Identity.Register;
using ABO.ToDoApp.DIC.Models;
using ABO.ToDoApp.Shared.Identity.Models;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ABO.ToDoApp.DIC.Controllers;

[ApiVersion("1.0")]
[Route("api/v1/Identity/Account")]
[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
public class IdentityController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("Register")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegisterUserResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(CustomProblemDetail))]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);

        return Ok(response);
    }

    [HttpPost("Login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(CustomProblemDetail))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CustomProblemDetail))]
    public async Task<IActionResult> Authenticate([FromBody] LoginUserRequest request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }
}
using ABO.ToDoApp.Application.Feautures.Identity;
using ABO.ToDoApp.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ABO.ToDoApp.DIC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);

            return Ok(response);
        }
    }
}

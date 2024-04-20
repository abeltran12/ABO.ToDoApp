﻿using ABO.ToDoApp.Application.Feautures.Identity.Login;
using ABO.ToDoApp.Application.Feautures.Identity.Register;
using ABO.ToDoApp.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

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

        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] LoginUserRequest request)
        {
            var response = await _mediator.Send(request);

            if(!response)
                return Unauthorized();
            //var tokenDto = await _service.AuthenticationService.CreateToken(populateExp: true);

            return Ok();
        }
    }
}

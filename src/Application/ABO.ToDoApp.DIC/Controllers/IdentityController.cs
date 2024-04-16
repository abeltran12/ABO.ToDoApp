using ABO.ToDoApp.Application;
using ABO.ToDoApp.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace ABO.ToDoApp.DIC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController(IAuthService service) : ControllerBase
    {
        private readonly IAuthService _service = service;

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            var response = await _service.RegisterUser(request);

            return Ok(response);
        }
    }
}

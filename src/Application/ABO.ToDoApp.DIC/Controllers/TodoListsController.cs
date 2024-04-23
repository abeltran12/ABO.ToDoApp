using ABO.ToDoApp.Application.Feautures.TodoList;
using ABO.ToDoApp.DIC.Models.TodoLists;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ABO.ToDoApp.DIC.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TodoListsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TodoListsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/<TodoListsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<TodoListsController>/5
        [HttpGet("{id:int}", Name = "GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok();
        }

        // POST api/<TodoListsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateTodoList request)
        {
            var response = await _mediator.Send(new CreateTodoListRequest { Name = request.Name });
            return CreatedAtRoute(nameof(GetById), new { id = response.Data!.Id, message = response.Message }, response);
        }

        // PUT api/<TodoListsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TodoListsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

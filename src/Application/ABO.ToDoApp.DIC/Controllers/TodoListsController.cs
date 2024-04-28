using ABO.ToDoApp.Application.Feautures.TodoList.Get;
using ABO.ToDoApp.DIC.Models.TodoLists;
using ABO.ToDoApp.Domain.RequestFilters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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
        public async Task<IActionResult> Get([FromQuery] SelectTodoListParameters parameters)
        {
            var (todoLists, metaData) = await _mediator.Send(
                new GetTodoListRequest 
                { 
                    Parameters = 
                        { 
                            PageSize = parameters.PageSize, 
                            PageNumber = parameters.PageNumber,
                            Name = parameters.Name,
                            Status = (Domain.Entities.Status)parameters.Status
                        }
                });

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(metaData));

            return Ok(todoLists);
        }

        // GET api/<TodoListsController>/5
        [HttpGet("{id:int}", Name = "GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok();
        }

        // POST api/<TodoListsController>
        //[HttpPost]
        //public async Task<IActionResult> Post([FromBody] CreateTodoList request)
        //{
        //    var response = await _mediator.Send(new CreateTodoListRequest { Name = request.Name });
        //    return CreatedAtRoute(nameof(GetById), new { id = response.Data!.Id, message = response.Message }, response);
        //}

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

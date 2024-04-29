using ABO.ToDoApp.Application.Feautures.TodoList.Create;
using ABO.ToDoApp.Application.Feautures.TodoList.Delete;
using ABO.ToDoApp.Application.Feautures.TodoList.Get;
using ABO.ToDoApp.Application.Feautures.TodoList.GetById;
using ABO.ToDoApp.Application.Feautures.TodoList.Update;
using ABO.ToDoApp.DIC.Models.TodoLists;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ABO.ToDoApp.DIC.Controllers
{
    [Authorize]
    [Route("api/todolists")]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _mediator.Send(new GetByIdTodoListRequest { Id = id });
            return Ok(response);
        }

        // POST api/<TodoListsController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Post([FromBody] CreateTodoList request)
        {
            var response = await _mediator.Send(
                    new CreateTodoListRequest 
                    { 
                        Name = request.Name,
                        Description = request.Description
                    });

            return CreatedAtRoute(nameof(GetById), new { id = response.Data!.Id, message = response.Message }, response);
        }

        // PUT api/<TodoListsController>/5
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateTodoList request)
        {
            var response = await _mediator.Send(
                    new UpdateTodoListRequest
                    {
                        Id = id,
                        Name = request.Name,
                        Description = request.Description
                    });

            return Ok(response);
        }

        // DELETE api/<TodoListsController>/5
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _mediator.Send(
                    new DeleteTodoListRequest
                    {
                        Id = id
                    });

            return Ok(response);
        }
    }
}
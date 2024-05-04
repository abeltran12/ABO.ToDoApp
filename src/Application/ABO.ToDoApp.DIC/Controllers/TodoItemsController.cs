using ABO.ToDoApp.Application.Feautures.TodoItem.Create;
using ABO.ToDoApp.Application.Feautures.TodoItem.Delete;
using ABO.ToDoApp.Application.Feautures.TodoItem.Get;
using ABO.ToDoApp.Application.Feautures.TodoItem.GetById;
using ABO.ToDoApp.Application.Feautures.TodoItem.Update;
using ABO.ToDoApp.Application.Feautures.TodoList.Delete;
using ABO.ToDoApp.Application.Feautures.TodoList.Update;
using ABO.ToDoApp.DIC.Models;
using ABO.ToDoApp.DIC.Models.TodoItems;
using ABO.ToDoApp.Shared.Models.TodoItem;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ABO.ToDoApp.DIC.Controllers
{
    [Authorize(Policy = "OwnerOfTodoListPolicy")]
    [Route("api/todolists/{todolistId}/todoitems")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TodoItemsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/<TodoItemsController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetTodoItemsListResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(CustomProblemDetail))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CustomProblemDetail))]
        public async Task<IActionResult> Get(int todolistId)
        {
            var response = 
                await _mediator.Send(new GetTodoItemRequest { TodolistId = todolistId });

            return Ok(response);
        }

        // GET api/<TodoItemsController>/5
        [HttpGet("{id:int}", Name = "GetTodoItemForTodoList")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetByIdTodoItemResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(CustomProblemDetail))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CustomProblemDetail))]
        public async Task<IActionResult> GetTodoItemForTodoList(int todolistId, int id)
        {
            var response = await _mediator.Send(
                new GetByIdTodoItemRequest 
                { 
                    Id = id, 
                    TodolistId = todolistId
                });

            return Ok(response);
        }

        // POST api/<TodoItemsController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateTodoItemResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(CustomProblemDetail))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CustomProblemDetail))]
        public async Task<IActionResult> Post(int todolistId,[FromBody] CreateTodoItem request)
        {
            var response = await _mediator.Send(
                    new CreateTodoItemRequest
                    {
                        Title = request.Title,
                        Description = request.Description,
                        Duedate = request.Duedate,
                        TodoListId = todolistId
                    });

            Response.Headers.Append("X-Message", JsonSerializer.Serialize(response.Message));

            return CreatedAtRoute(nameof(GetTodoItemForTodoList), new { todolistId, id = response.Data!.Id }, response.Data);
        }

        // PUT api/<TodoItemsController>/5
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(CustomProblemDetail))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CustomProblemDetail))]
        public async Task<IActionResult> Put(int todolistId, int id, [FromBody] UpdateTodoItem request)
        {
            var response = await _mediator.Send(
                    new UpdateTodoItemRequest
                    {
                        Id = id,
                        Title = request.Title,
                        Description = request.Description,
                        Duedate = request.Duedate,
                        Status = (Domain.Entities.Status)request.Status,
                        TodoListId = todolistId
                    });

            Response.Headers.Append("X-Message", JsonSerializer.Serialize(response));

            return NoContent();
        }

        // DELETE api/<TodoItemsController>/5
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(CustomProblemDetail))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CustomProblemDetail))]
        public async Task<IActionResult> Delete(int todolistId, int id)
        {
            var response = await _mediator.Send(
                    new DeleteTodoItemRequest
                    {
                        Id = id,
                        TodolistId = todolistId
                    });

            Response.Headers.Append("X-Message", JsonSerializer.Serialize(response));

            return NoContent();
        }
    }
}
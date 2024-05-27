using ABO.ToDoApp.Application.Feautures.TodoList.Create;
using ABO.ToDoApp.Application.Feautures.TodoList.Delete;
using ABO.ToDoApp.Application.Feautures.TodoList.Get;
using ABO.ToDoApp.Application.Feautures.TodoList.GetById;
using ABO.ToDoApp.Application.Feautures.TodoList.Update;
using ABO.ToDoApp.DIC.Models;
using ABO.ToDoApp.DIC.Models.TodoLists;
using ABO.ToDoApp.Domain.RequestFilters;
using ABO.ToDoApp.Shared.Models.TodoList;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ABO.ToDoApp.DIC.Controllers;

[ApiVersion("1.0")]
[Authorize]
[Route("api/v1/Todolists")]
[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
public class TodoListsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    // GET: api/<TodoListsController>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedList<GetAllTodoListResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(CustomProblemDetail))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(void))]
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetByIdTodoListResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(CustomProblemDetail))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CustomProblemDetail))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(void))]
    public async Task<IActionResult> GetById(int id)
    {
        var response = await _mediator.Send(new GetByIdTodoListRequest { Id = id });
        return Ok(response);
    }

    // POST api/<TodoListsController>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateTodoListResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(CustomProblemDetail))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CustomProblemDetail))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(void))]
    public async Task<IActionResult> Post([FromBody] CreateTodoList request)
    {
        var response = await _mediator.Send(
                new CreateTodoListRequest 
                { 
                    Name = request.Name,
                    Description = request.Description
                });

        Response.Headers.Append("X-Message", JsonSerializer.Serialize(response.Message));

        return CreatedAtRoute(nameof(GetById), new { id = response.Data!.Id }, response.Data);
    }

    // PUT api/<TodoListsController>/5
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(CustomProblemDetail))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CustomProblemDetail))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(void))]
    public async Task<IActionResult> Put(int id, [FromBody] UpdateTodoList request)
    {
        var response = await _mediator.Send(
                new UpdateTodoListRequest
                {
                    Id = id,
                    Name = request.Name,
                    Description = request.Description
                });

        Response.Headers.Append("X-Message", JsonSerializer.Serialize(response));

        return NoContent();
    }

    // DELETE api/<TodoListsController>/5
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(CustomProblemDetail))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CustomProblemDetail))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(void))]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _mediator.Send(
                new DeleteTodoListRequest
                {
                    Id = id
                });

        Response.Headers.Append("X-Message", JsonSerializer.Serialize(response));

        return NoContent();
    }
}
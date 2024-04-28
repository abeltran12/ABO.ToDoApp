using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ABO.ToDoApp.DIC.Controllers;

//[Authorize]
//[Route("api/todolists/{todoListId}/todoitems")]
//[ApiController]
//public class TodoItemsController : ControllerBase
//{
//    private readonly IMediator _mediator;

//    public TodoItemsController(IMediator mediator)
//    {
//        _mediator = mediator;
//    }

//    //[HttpGet(Name = "GetById")]
//    //public async Task<IActionResult> GetById(int todoListId)
//    //{
//    //    return Ok();
//    //}

//    //// POST api/<TodoItemsController>
//    //[HttpPost]
//    //public async Task<IActionResult> Post(int todoListId, [FromBody] CreateTodoItem request)
//    //{
//    //    var response = await _mediator.Send(
//    //        new CreateTodoItemRequest 
//    //        { 
//    //            Title = request.Title,
//    //            Description = request.Description,
//    //            Duedate = request.Duedate,
//    //            TodoListId = todoListId
//    //        });
//    //    return CreatedAtRoute(nameof(GetById), new { id = response.Data!.Id, message = response.Message }, response);
//    //}
//}

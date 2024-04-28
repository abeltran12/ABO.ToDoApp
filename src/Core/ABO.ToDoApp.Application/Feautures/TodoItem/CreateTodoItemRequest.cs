using ABO.ToDoApp.Shared.Models.TodoItem;
using ABO.ToDoApp.Shared.Models.TodoList;
using MediatR;

namespace ABO.ToDoApp.Application.Feautures.TodoItem;

public class CreateTodoItemRequest : IRequest<ActionsResponse<CreateTodoItemResponse>>
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateOnly Duedate { get; set; }

    public int TodoListId { get; set; }
}

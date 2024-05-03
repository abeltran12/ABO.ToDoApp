using ABO.ToDoApp.Domain.Entities;
using MediatR;

namespace ABO.ToDoApp.Application.Feautures.TodoItem.Update;

public class UpdateTodoItemRequest : IRequest<string>
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateOnly Duedate { get; set; }

    public Status Status { get; set; }

    public int TodoListId { get; set; }
}

using ABO.ToDoApp.Shared.Models.TodoList;
using MediatR;

namespace ABO.ToDoApp.Application.Feautures.TodoList.Update;

public class UpdateTodoListRequest : IRequest<ActionsResponse<bool>>
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }
}
using MediatR;

namespace ABO.ToDoApp.Application.Feautures.TodoList.Update;

public class UpdateTodoListRequest : IRequest<string>
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }
}
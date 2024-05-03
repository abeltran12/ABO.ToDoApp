using MediatR;

namespace ABO.ToDoApp.Application.Feautures.TodoList.Delete;

public class DeleteTodoListRequest : IRequest<string>
{
    public int Id { get; set; }
}
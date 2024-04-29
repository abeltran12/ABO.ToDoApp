using ABO.ToDoApp.Shared.Models.TodoList;
using MediatR;

namespace ABO.ToDoApp.Application.Feautures.TodoList.Delete;

public class DeleteTodoListRequest : IRequest<ActionsResponse<bool>>
{
    public int Id { get; set; }
}
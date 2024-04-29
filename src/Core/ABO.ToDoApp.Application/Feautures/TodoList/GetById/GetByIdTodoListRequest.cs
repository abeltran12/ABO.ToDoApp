using ABO.ToDoApp.Shared.Models.TodoList;
using MediatR;

namespace ABO.ToDoApp.Application.Feautures.TodoList.GetById;

public class GetByIdTodoListRequest : IRequest<GetByIdTodoListResponse>
{
    public int Id { get; set; }
}

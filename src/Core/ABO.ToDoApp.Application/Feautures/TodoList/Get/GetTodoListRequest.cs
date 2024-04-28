using ABO.ToDoApp.Domain.RequestFilters;
using ABO.ToDoApp.Shared.Models.TodoList;
using MediatR;

namespace ABO.ToDoApp.Application.Feautures.TodoList.Get;

public class GetTodoListRequest : IRequest<(IEnumerable<GetAllTodoListResponse> todoListResponses, MetaData metaData)>
{
    public TodoListParameters Parameters { get; set; } = new();
}

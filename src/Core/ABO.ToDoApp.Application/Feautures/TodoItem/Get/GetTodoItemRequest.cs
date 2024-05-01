using ABO.ToDoApp.Shared.Models.TodoItem;
using MediatR;

namespace ABO.ToDoApp.Application.Feautures.TodoItem.Get;

public class GetTodoItemRequest : IRequest<IEnumerable<GetTodoItemsListResponse>>
{
    public int TodolistId { get; set; }
}

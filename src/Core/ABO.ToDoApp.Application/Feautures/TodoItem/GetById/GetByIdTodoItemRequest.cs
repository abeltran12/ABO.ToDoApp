using ABO.ToDoApp.Shared.Models.TodoItem;
using MediatR;

namespace ABO.ToDoApp.Application.Feautures.TodoItem.GetById;

public class GetByIdTodoItemRequest : IRequest<GetByIdTodoItemResponse>
{
    public int Id { get; set; }

    public int TodolistId { get; set; }
}

using MediatR;

namespace ABO.ToDoApp.Application.Feautures.TodoItem.Delete;

public class DeleteTodoItemRequest : IRequest<string>
{
    public int Id { get; set; }

    public int TodolistId { get; set; }
}

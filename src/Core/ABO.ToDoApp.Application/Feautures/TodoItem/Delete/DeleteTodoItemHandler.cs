using ABO.ToDoApp.Application.Exceptions;
using ABO.ToDoApp.Domain.Repositories;
using ABO.ToDoApp.Shared.Constants.TodoItems;
using MediatR;

namespace ABO.ToDoApp.Application.Feautures.TodoItem.Delete;

public class DeleteTodoItemHandler : IRequestHandler<DeleteTodoItemRequest, string>
{
    private readonly IUnitofwork _unitofwork;

    public DeleteTodoItemHandler(IUnitofwork unitofwork)
    {
        _unitofwork = unitofwork;
    }

    public async Task<string> Handle(DeleteTodoItemRequest request, CancellationToken cancellationToken)
    {
        var response = 
            await _unitofwork.TodoItemRepository.GetByIdAsync(request.TodolistId, request.Id);

        if (response == null)
            throw new NotFoundException("TodoItem", request.Id);

        response.Status = Domain.Entities.Status.Deleted;

        _unitofwork.TodoItemRepository.Update(response);
        await _unitofwork.SaveAsync();

        return TodoItemMessageConstants.SuccessMessage;
    }
}
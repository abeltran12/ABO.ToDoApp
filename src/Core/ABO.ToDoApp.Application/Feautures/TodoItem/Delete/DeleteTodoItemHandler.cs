using ABO.ToDoApp.Application.Exceptions;
using ABO.ToDoApp.Domain.Repositories;
using ABO.ToDoApp.Shared.Constants.GenericMessages;
using ABO.ToDoApp.Shared.Constants.TodoItems;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABO.ToDoApp.Application.Feautures.TodoItem.Delete;

public class DeleteTodoItemHandler : IRequestHandler<DeleteTodoItemRequest, string>
{
    private readonly IUnitofwork _unitofwork;
    private readonly ILogger<DeleteTodoItemHandler> _logger;

    public DeleteTodoItemHandler(IUnitofwork unitofwork, ILogger<DeleteTodoItemHandler> logger)
    {
        _unitofwork = unitofwork;
        _logger = logger;
    }

    public async Task<string> Handle(DeleteTodoItemRequest request, CancellationToken cancellationToken)
    {
        _logger
            .LogInformation(GenericMessageConstants.ExecutingMessage
                + " {Request}", nameof(DeleteTodoItemHandler));

        var response = 
            await _unitofwork.TodoItemRepository.GetByIdAsync(request.TodolistId, request.Id);

        if (response == null)
            throw new NotFoundException("TodoItem", request.Id);

        response.Status = Domain.Entities.Status.Deleted;

        _unitofwork.TodoItemRepository.Update(response);
        await _unitofwork.SaveAsync();

        _logger.LogInformation(GenericMessageConstants.ExecutingMessage
                + " {Request} " + GenericMessageConstants.ProcessedMessage, nameof(DeleteTodoItemHandler));

        return TodoItemMessageConstants.SuccessMessage;
    }
}

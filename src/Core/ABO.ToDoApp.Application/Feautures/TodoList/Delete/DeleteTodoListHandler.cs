using ABO.ToDoApp.Application.Exceptions;
using ABO.ToDoApp.Domain.Repositories;
using ABO.ToDoApp.Shared.Constants.GenericMessages;
using ABO.ToDoApp.Shared.Constants.TodoLists;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABO.ToDoApp.Application.Feautures.TodoList.Delete;

public class DeleteTodoListHandler : IRequestHandler<DeleteTodoListRequest, string>
{
    private readonly IUnitofwork _unitofwork;
    private readonly ILogger<DeleteTodoListHandler> _logger;

    public DeleteTodoListHandler(IUnitofwork unitofwork, ILogger<DeleteTodoListHandler> logger)
    {
        _unitofwork = unitofwork;
        _logger = logger;
    }

    public async Task<string> Handle(DeleteTodoListRequest request, CancellationToken cancellationToken)
    {
        _logger
            .LogInformation(GenericMessageConstants.ExecutingMessage
                + " {Request}", nameof(DeleteTodoListHandler));

        var response = await _unitofwork.TodoListRepository.GetByIdAsync(request.Id);

        if (response == null)
            throw new NotFoundException("TodoList", request.Id);

        response.Status = Domain.Entities.Status.Deleted;

        _unitofwork.TodoListRepository.Update(response);
        await DeleteTodoItems(request.Id);
        await _unitofwork.SaveAsync();

        _logger.LogInformation(GenericMessageConstants.ExecutingMessage
            + " {Request} " + GenericMessageConstants.ProcessedMessage, nameof(DeleteTodoListHandler));

        return TodoListMessageConstants.SuccessMessage;
    }

    private async Task DeleteTodoItems(int todoListId)
    {
        var todoItems = await _unitofwork.TodoItemRepository.GetAllForUpdate(todoListId);

        todoItems.ForEach(item => 
                { 
                    item.Status = Domain.Entities.Status.Deleted;
                });

        _unitofwork.TodoItemRepository.UpdateAll(todoItems);

        return;
    }
}
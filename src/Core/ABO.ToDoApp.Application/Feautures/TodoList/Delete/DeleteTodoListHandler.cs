using ABO.ToDoApp.Application.Exceptions;
using ABO.ToDoApp.Domain.Repositories;
using ABO.ToDoApp.Shared.Constants.TodoLists;
using MediatR;

namespace ABO.ToDoApp.Application.Feautures.TodoList.Delete;

public class DeleteTodoListHandler(IUnitofwork unitofwork) : IRequestHandler<DeleteTodoListRequest, string>
{
    private readonly IUnitofwork _unitofwork = unitofwork;

    public async Task<string> Handle(DeleteTodoListRequest request, CancellationToken cancellationToken)
    {
        var response = await _unitofwork.TodoListRepository.GetByIdAsync(request.Id) 
            ?? throw new NotFoundException("TodoList", request.Id);

        response.Status = Domain.Entities.Status.Deleted;

        _unitofwork.TodoListRepository.Update(response);
        await DeleteTodoItems(request.Id);
        await _unitofwork.SaveAsync();

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
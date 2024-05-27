using ABO.ToDoApp.Application.Exceptions;
using ABO.ToDoApp.Domain.Entities;
using ABO.ToDoApp.Domain.Repositories;
using ABO.ToDoApp.Shared.Constants.TodoItems;
using AutoMapper;
using MediatR;

namespace ABO.ToDoApp.Application.Feautures.TodoItem.Update;

public class UpdateTodoItemHandler(IMapper mapper, IUnitofwork unitofwork) : IRequestHandler<UpdateTodoItemRequest, string>
{
    private readonly IMapper _mapper = mapper;
    private readonly IUnitofwork _unitofwork = unitofwork;

    public async Task<string> Handle(UpdateTodoItemRequest request, CancellationToken cancellationToken)
    {
        Domain.Entities.TodoItem response = await ValidateExistenceAndStatusRule(request);

        _mapper.Map(request, response);
        _unitofwork.TodoItemRepository.Update(response);

        await ReviewStatusCompleted(request);

        await _unitofwork.SaveAsync();

        return TodoItemMessageConstants.SuccessMessage;
    }

    private async Task<Domain.Entities.TodoItem> ValidateExistenceAndStatusRule(UpdateTodoItemRequest request)
    {
        var response = await _unitofwork.TodoItemRepository
            .GetByIdAsync(request.TodoListId, request.Id) ?? 
                throw new NotFoundException("TodoItem", request.Id);

        if (response.Status.Equals(Status.Completed) 
                && request.Status.Equals(Status.Active))
            throw new BadRequestException("Cant reopen a completed activity.");

        return response;
    }

    private async Task ReviewStatusCompleted(UpdateTodoItemRequest request)
    {
        if (request.Status.Equals(Status.Completed))
        {
            int itemsCompleted =
                await _unitofwork.TodoItemRepository.GetTodoItemsCompletedCount(request.TodoListId);

            if (itemsCompleted + 1 == 10)
            {
                var todoList = await _unitofwork.TodoListRepository.GetByIdAsync(request.TodoListId);
                todoList!.Status = Status.Completed;
                _unitofwork.TodoListRepository.Update(todoList);
            }
        }
    }
}

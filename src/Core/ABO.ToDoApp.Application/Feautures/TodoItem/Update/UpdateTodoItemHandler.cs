using ABO.ToDoApp.Application.Exceptions;
using ABO.ToDoApp.Domain.Entities;
using ABO.ToDoApp.Domain.Repositories;
using ABO.ToDoApp.Shared.Constants.TodoItems;
using AutoMapper;
using MediatR;

namespace ABO.ToDoApp.Application.Feautures.TodoItem.Update;

public class UpdateTodoItemHandler : IRequestHandler<UpdateTodoItemRequest, string>
{
    private readonly IMapper _mapper;
    private readonly IUnitofwork _unitofwork;

    public UpdateTodoItemHandler(IMapper mapper, IUnitofwork unitofwork)
    {
        _mapper = mapper;
        _unitofwork = unitofwork;
    }

    public async Task<string> Handle(UpdateTodoItemRequest request, CancellationToken cancellationToken)
    {
        await ModelValidations(request, cancellationToken);
        Domain.Entities.TodoItem response = await ValidateExistenceAndStatusRule(request);

        _mapper.Map(request, response);
        _unitofwork.TodoItemRepository.Update(response);

        //despues de aca busco a ver si aparece actualizado o no
        //hago el count y si da 10 acutalizo todo
        //Aca debo incluir si o si el cancellation token
        //las listas completadas no se editan, colocar eso en el otro update
        await ReviewStatusCompleted(request);

        await _unitofwork.SaveAsync();

        return TodoItemMessageConstants.SuccessMessage;
    }

    private async Task<Domain.Entities.TodoItem> ValidateExistenceAndStatusRule(UpdateTodoItemRequest request)
    {
        var response =
                    await _unitofwork.TodoItemRepository.GetByIdAsync(request.TodoListId, request.Id);

        if (response == null)
            throw new NotFoundException("TodoItem", request.Id);

        if (response.Status.Equals(Status.Completed) 
                && request.Status.Equals(Status.Active))
            throw new BadRequestException("Cant reopen a completed activity.");

        return response;
    }

    private static async Task ModelValidations(UpdateTodoItemRequest request, CancellationToken cancellationToken)
    {
        var validator = new UpdateTodoItemRequestValidator();
        var validatorResult = await validator.ValidateAsync(request, cancellationToken);

        if (validatorResult.Errors.Count != 0)
            throw new BadRequestException(TodoItemMessageConstants.ErrorMessage, validatorResult);
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

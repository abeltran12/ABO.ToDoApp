using ABO.ToDoApp.Application.Exceptions;
using ABO.ToDoApp.Domain.Repositories;
using ABO.ToDoApp.Shared.Constants.TodoItems;
using ABO.ToDoApp.Shared.Constants.TodoLists;
using ABO.ToDoApp.Shared.Models.TodoItem;
using ABO.ToDoApp.Shared.Models.TodoList;
using AutoMapper;
using MediatR;

namespace ABO.ToDoApp.Application.Feautures.TodoItem.Create;

public class CreateTodoItemHandler : IRequestHandler<CreateTodoItemRequest, ActionsResponse<CreateTodoItemResponse>>
{
    private readonly IMapper _mapper;
    private readonly IUnitofwork _unitofwork;

    public CreateTodoItemHandler(IMapper mapper, IUnitofwork unitofwork)
    {
        _mapper = mapper;
        _unitofwork = unitofwork;
    }

    public async Task<ActionsResponse<CreateTodoItemResponse>> Handle(CreateTodoItemRequest request, CancellationToken cancellationToken)
    {
        await Validations(request, cancellationToken);

        var todoItem = _mapper.Map<Domain.Entities.TodoItem>(request);

        await _unitofwork.TodoItemRepository.CreateAsync(todoItem);
        await _unitofwork.SaveAsync();

        var responseData = _mapper.Map<CreateTodoItemResponse>(todoItem);

        return new ActionsResponse<CreateTodoItemResponse>
        {
            Data = responseData,
            Message = TodoItemMessageConstants.TodoListCreatedSuccessfully
        };
    }

    private async Task Validations(CreateTodoItemRequest request, CancellationToken cancellationToken)
    {
        var validator = new CreateTodoItemRequestValidator();
        var validatorResult = await validator.ValidateAsync(request, cancellationToken);

        if (validatorResult.Errors.Count != 0)
            throw new BadRequestException(TodoListMessageConstants.ErrorMessage, validatorResult);

        if (await _unitofwork.TodoItemRepository.GetTodoItemsCount(request.TodoListId) >= 10)
            throw new BadRequestException("The maximum allowed number of items has been reached.");
    }
}

using ABO.ToDoApp.Application.Exceptions;
using ABO.ToDoApp.Domain.Repositories;
using ABO.ToDoApp.Shared.Constants.GenericMessages;
using ABO.ToDoApp.Shared.Constants.TodoItems;
using ABO.ToDoApp.Shared.Constants.TodoLists;
using ABO.ToDoApp.Shared.Models.TodoItem;
using ABO.ToDoApp.Shared.Models.TodoList;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABO.ToDoApp.Application.Feautures.TodoItem.Create;

public class CreateTodoItemHandler : IRequestHandler<CreateTodoItemRequest, ActionsResponse<CreateTodoItemResponse>>
{
    private readonly IMapper _mapper;
    private readonly IUnitofwork _unitofwork;
    private readonly ILogger<CreateTodoItemHandler> _logger;

    public CreateTodoItemHandler(IMapper mapper, IUnitofwork unitofwork, ILogger<CreateTodoItemHandler> logger)
    {
        _mapper = mapper;
        _unitofwork = unitofwork;
        _logger = logger;
    }

    public async Task<ActionsResponse<CreateTodoItemResponse>> Handle(CreateTodoItemRequest request, CancellationToken cancellationToken)
    {
        _logger
            .LogInformation(GenericMessageConstants.ExecutingMessage
                + " {Request}", nameof(CreateTodoItemHandler));

        await Validations(request, cancellationToken);

        var todoItem = _mapper.Map<Domain.Entities.TodoItem>(request);

        await _unitofwork.TodoItemRepository.CreateAsync(todoItem);
        await _unitofwork.SaveAsync();

        var responseData = _mapper.Map<CreateTodoItemResponse>(todoItem);

        _logger.LogInformation(GenericMessageConstants.ExecutingMessage
                + " {Request} " + GenericMessageConstants.ProcessedMessage, nameof(CreateTodoItemHandler));

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

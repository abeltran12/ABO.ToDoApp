﻿using ABO.ToDoApp.Application.Exceptions;
using ABO.ToDoApp.Domain.Repositories;
using ABO.ToDoApp.Shared.Constants.TodoItems;
using ABO.ToDoApp.Shared.Models.TodoItem;
using ABO.ToDoApp.Shared.Models.TodoList;
using AutoMapper;
using MediatR;

namespace ABO.ToDoApp.Application.Feautures.TodoItem.Create;

public class CreateTodoItemHandler(IMapper mapper, IUnitofwork unitofwork) : IRequestHandler<CreateTodoItemRequest, ActionsResponse<CreateTodoItemResponse>>
{
    private readonly IMapper _mapper = mapper;
    private readonly IUnitofwork _unitofwork = unitofwork;

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
            Message = TodoItemMessageConstants.TodoItemCreatedSuccessfully
        };
    }

    private async Task Validations(CreateTodoItemRequest request, CancellationToken cancellationToken)
    {
        if (await _unitofwork.TodoItemRepository.GetTodoItemsCount(request.TodoListId) >= 10)
            throw new BadRequestException("The maximum allowed number of items has been reached.");
    }
}

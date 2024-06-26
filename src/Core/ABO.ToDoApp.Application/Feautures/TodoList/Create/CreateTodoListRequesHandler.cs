﻿using ABO.ToDoApp.Domain.Repositories;
using ABO.ToDoApp.Shared.Constants.TodoLists;
using ABO.ToDoApp.Shared.Identity.Models;
using ABO.ToDoApp.Shared.Models.TodoList;
using AutoMapper;
using MediatR;

namespace ABO.ToDoApp.Application.Feautures.TodoList.Create;

public class CreateTodoListRequesHandler(IMapper mapper,
    IUnitofwork unitofwork,
    IdentityConfig identityConfig) : IRequestHandler<CreateTodoListRequest, ActionsResponse<CreateTodoListResponse>>
{
    private readonly IMapper _mapper = mapper;
    private readonly IUnitofwork _unitofwork = unitofwork;
    private readonly IdentityConfig _identityConfig = identityConfig;

    public async Task<ActionsResponse<CreateTodoListResponse>> Handle(CreateTodoListRequest request, CancellationToken cancellationToken)
    {
        var todoList = _mapper.Map<Domain.Entities.TodoList>(request);
        todoList.UserId = _identityConfig.UserId!;

        await _unitofwork.TodoListRepository.CreateAsync(todoList);
        await _unitofwork.SaveAsync();

        var responseData = _mapper.Map<CreateTodoListResponse>(todoList);

        return new ActionsResponse<CreateTodoListResponse>
        {
            Data = responseData,
            Message = TodoListMessageConstants.TodoListCreatedSuccessfully
        };
    }
}
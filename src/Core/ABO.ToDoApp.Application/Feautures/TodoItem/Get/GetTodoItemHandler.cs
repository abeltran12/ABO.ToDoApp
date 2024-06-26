﻿using ABO.ToDoApp.Domain.Repositories;
using ABO.ToDoApp.Shared.Models.TodoItem;
using AutoMapper;
using MediatR;

namespace ABO.ToDoApp.Application.Feautures.TodoItem.Get;

public class GetTodoItemHandler(IMapper mapper, IUnitofwork unitofwork) : IRequestHandler<GetTodoItemRequest, IEnumerable<GetTodoItemsListResponse>>
{
    private readonly IMapper _mapper = mapper;
    private readonly IUnitofwork _unitofwork = unitofwork;

    public async Task<IEnumerable<GetTodoItemsListResponse>> Handle(GetTodoItemRequest request, CancellationToken cancellationToken)
    {
        var response = await _unitofwork.TodoItemRepository.GetAll(request.TodolistId);
        var mapResponse = _mapper.Map<IEnumerable<GetTodoItemsListResponse>>(response);

        return mapResponse;
    }
}
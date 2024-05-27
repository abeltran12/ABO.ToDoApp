using ABO.ToDoApp.Application.Exceptions;
using ABO.ToDoApp.Domain.Repositories;
using ABO.ToDoApp.Shared.Models.TodoItem;
using AutoMapper;
using MediatR;

namespace ABO.ToDoApp.Application.Feautures.TodoItem.GetById;

public class GetByIdTodoItemHandler(IMapper mapper, IUnitofwork unitofwork) : IRequestHandler<GetByIdTodoItemRequest, GetByIdTodoItemResponse>
{
    private readonly IMapper _mapper = mapper;
    private readonly IUnitofwork _unitofwork = unitofwork;

    public async Task<GetByIdTodoItemResponse> Handle(GetByIdTodoItemRequest request, CancellationToken cancellationToken)
    {
        var response = 
            await _unitofwork.TodoItemRepository.GetByIdAsync(request.TodolistId, request.Id) 
                ?? throw new NotFoundException("TodoItem", request.Id);

        var responseMap = _mapper.Map<GetByIdTodoItemResponse>(response);

        return responseMap;
    }

}
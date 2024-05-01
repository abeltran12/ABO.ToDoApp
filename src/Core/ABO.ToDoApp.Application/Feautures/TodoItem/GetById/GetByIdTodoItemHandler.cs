using ABO.ToDoApp.Application.Exceptions;
using ABO.ToDoApp.Domain.Repositories;
using ABO.ToDoApp.Shared.Models.TodoItem;
using AutoMapper;
using MediatR;

namespace ABO.ToDoApp.Application.Feautures.TodoItem.GetById;

public class GetByIdTodoItemHandler : IRequestHandler<GetByIdTodoItemRequest, GetByIdTodoItemResponse>
{
    private readonly IMapper _mapper;
    private readonly IUnitofwork _unitofwork;

    public GetByIdTodoItemHandler(IMapper mapper, IUnitofwork unitofwork)
    {
        _mapper = mapper;
        _unitofwork = unitofwork;
    }

    public async Task<GetByIdTodoItemResponse> Handle(GetByIdTodoItemRequest request, CancellationToken cancellationToken)
    {
        var response = await _unitofwork.TodoItemRepository.GetByIdAsync(request.Id);

        if (response == null)
            throw new NotFoundException("TodoItem", request.Id);

        var responseMap = _mapper.Map<GetByIdTodoItemResponse>(response);

        return responseMap;
    }

}
using ABO.ToDoApp.Application.Exceptions;
using ABO.ToDoApp.Domain.Repositories;
using ABO.ToDoApp.Shared.Models.TodoList;
using AutoMapper;
using MediatR;

namespace ABO.ToDoApp.Application.Feautures.TodoList.GetById;

public class GetByIdTodoListHandler : IRequestHandler<GetByIdTodoListRequest, GetByIdTodoListResponse>
{
    private readonly IMapper _mapper;
    private readonly IUnitofwork _unitofwork;

    public GetByIdTodoListHandler(IMapper mapper, IUnitofwork unitofwork)
    {
        _mapper = mapper;
        _unitofwork = unitofwork;
    }

    public async Task<GetByIdTodoListResponse> Handle(GetByIdTodoListRequest request, CancellationToken cancellationToken)
    {
        var response = await _unitofwork.TodoListRepository.GetByIdAsync(request.Id);

        if (response == null)
            throw new NotFoundException("TodoList", request.Id);

        var responseMap = _mapper.Map<GetByIdTodoListResponse>(response);

        return responseMap;
    }
}
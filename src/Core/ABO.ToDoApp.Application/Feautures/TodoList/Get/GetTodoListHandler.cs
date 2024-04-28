using ABO.ToDoApp.Domain.Repositories;
using ABO.ToDoApp.Domain.RequestFilters;
using ABO.ToDoApp.Shared.Models.TodoList;
using AutoMapper;
using MediatR;

namespace ABO.ToDoApp.Application.Feautures.TodoList.Get;

public class GetTodoListHandler : IRequestHandler<GetTodoListRequest, (IEnumerable<GetAllTodoListResponse> todoListResponses, MetaData metaData)>
{
    private readonly IMapper _mapper;
    private readonly IUnitofwork _unitofwork;

    public GetTodoListHandler(IMapper mapper, IUnitofwork unitofwork)
    {
        _mapper = mapper;
        _unitofwork = unitofwork;
    }

    public async Task<(IEnumerable<GetAllTodoListResponse> todoListResponses, MetaData metaData)> Handle(GetTodoListRequest request, CancellationToken cancellationToken)
    {
        var response = await _unitofwork.TodoListRepository.GetAll(request.Parameters);

        var mapResponse = _mapper.Map<IEnumerable<GetAllTodoListResponse>>(response);

        return (todoListResponses: mapResponse, metaData: response.MetaData);
    }
}
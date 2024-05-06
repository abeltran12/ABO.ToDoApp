using ABO.ToDoApp.Domain.Repositories;
using ABO.ToDoApp.Domain.RequestFilters;
using ABO.ToDoApp.Shared.Constants.GenericMessages;
using ABO.ToDoApp.Shared.Models.TodoList;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABO.ToDoApp.Application.Feautures.TodoList.Get;

public class GetTodoListHandler : IRequestHandler<GetTodoListRequest, (IEnumerable<GetAllTodoListResponse> todoListResponses, MetaData metaData)>
{
    private readonly IMapper _mapper;
    private readonly IUnitofwork _unitofwork;
    private readonly ILogger<GetTodoListHandler> _logger;

    public GetTodoListHandler(IMapper mapper, IUnitofwork unitofwork, ILogger<GetTodoListHandler> logger)
    {
        _mapper = mapper;
        _unitofwork = unitofwork;
        _logger = logger;
    }

    public async Task<(IEnumerable<GetAllTodoListResponse> todoListResponses, MetaData metaData)> Handle(GetTodoListRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(GenericMessageConstants.ExecutingMessage
            + " {Request}", nameof(GetTodoListHandler));

        var response = await _unitofwork.TodoListRepository.GetAll(request.Parameters);

        var mapResponse = _mapper.Map<IEnumerable<GetAllTodoListResponse>>(response);

        _logger.LogInformation(GenericMessageConstants.ExecutingMessage
            + " {Request} " + GenericMessageConstants.ProcessedMessage, nameof(GetTodoListHandler));

        return (todoListResponses: mapResponse, metaData: response.MetaData);
    }
}
using ABO.ToDoApp.Application.Exceptions;
using ABO.ToDoApp.Domain.Repositories;
using ABO.ToDoApp.Shared.Constants.GenericMessages;
using ABO.ToDoApp.Shared.Models.TodoList;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABO.ToDoApp.Application.Feautures.TodoList.GetById;

public class GetByIdTodoListHandler : IRequestHandler<GetByIdTodoListRequest, GetByIdTodoListResponse>
{
    private readonly IMapper _mapper;
    private readonly IUnitofwork _unitofwork;
    private readonly ILogger<GetByIdTodoListHandler> _logger;

    public GetByIdTodoListHandler(IMapper mapper, IUnitofwork unitofwork, ILogger<GetByIdTodoListHandler> logger)
    {
        _mapper = mapper;
        _unitofwork = unitofwork;
        _logger = logger;
    }

    public async Task<GetByIdTodoListResponse> Handle(GetByIdTodoListRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(GenericMessageConstants.ExecutingMessage
            + " {Request}", nameof(GetByIdTodoListHandler));

        var response = await _unitofwork.TodoListRepository.GetByIdAsync(request.Id);

        if (response == null)
            throw new NotFoundException("TodoList", request.Id);

        var responseMap = _mapper.Map<GetByIdTodoListResponse>(response);

        _logger.LogInformation(GenericMessageConstants.ExecutingMessage
            + " {Request} " + GenericMessageConstants.ProcessedMessage, nameof(GetByIdTodoListHandler));

        return responseMap;
    }
}
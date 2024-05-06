using ABO.ToDoApp.Domain.Repositories;
using ABO.ToDoApp.Shared.Constants.GenericMessages;
using ABO.ToDoApp.Shared.Models.TodoItem;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABO.ToDoApp.Application.Feautures.TodoItem.Get;

public class GetTodoItemHandler : IRequestHandler<GetTodoItemRequest, IEnumerable<GetTodoItemsListResponse>>
{
    private readonly IMapper _mapper;
    private readonly IUnitofwork _unitofwork;
    private readonly ILogger<GetTodoItemHandler> _logger;

    public GetTodoItemHandler(IMapper mapper, IUnitofwork unitofwork, ILogger<GetTodoItemHandler> logger)
    {
        _mapper = mapper;
        _unitofwork = unitofwork;
        _logger = logger;
    }

    public async Task<IEnumerable<GetTodoItemsListResponse>> Handle(GetTodoItemRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(GenericMessageConstants.ExecutingMessage
            + " {Request}", nameof(GetTodoItemHandler));

        var response = await _unitofwork.TodoItemRepository.GetAll(request.TodolistId);

        var mapResponse = _mapper.Map<IEnumerable<GetTodoItemsListResponse>>(response);

        _logger.LogInformation(GenericMessageConstants.ExecutingMessage
            + " {Request} " + GenericMessageConstants.ProcessedMessage, nameof(GetTodoItemHandler));

        return mapResponse;
    }
}

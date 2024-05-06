using ABO.ToDoApp.Application.Exceptions;
using ABO.ToDoApp.Domain.Repositories;
using ABO.ToDoApp.Shared.Constants.GenericMessages;
using ABO.ToDoApp.Shared.Models.TodoItem;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABO.ToDoApp.Application.Feautures.TodoItem.GetById;

public class GetByIdTodoItemHandler : IRequestHandler<GetByIdTodoItemRequest, GetByIdTodoItemResponse>
{
    private readonly IMapper _mapper;
    private readonly IUnitofwork _unitofwork;
    private readonly ILogger<GetByIdTodoItemHandler> _logger;

    public GetByIdTodoItemHandler(IMapper mapper, IUnitofwork unitofwork, ILogger<GetByIdTodoItemHandler> logger)
    {
        _mapper = mapper;
        _unitofwork = unitofwork;
        _logger = logger;
    }

    public async Task<GetByIdTodoItemResponse> Handle(GetByIdTodoItemRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(GenericMessageConstants.ExecutingMessage
            + " {Request}", nameof(GetByIdTodoItemHandler));

        var response = 
            await _unitofwork.TodoItemRepository.GetByIdAsync(request.TodolistId, request.Id);

        if (response == null)
            throw new NotFoundException("TodoItem", request.Id);

        var responseMap = _mapper.Map<GetByIdTodoItemResponse>(response);

        _logger.LogInformation(GenericMessageConstants.ExecutingMessage
            + " {Request} " + GenericMessageConstants.ProcessedMessage, nameof(GetByIdTodoItemHandler));

        return responseMap;
    }

}
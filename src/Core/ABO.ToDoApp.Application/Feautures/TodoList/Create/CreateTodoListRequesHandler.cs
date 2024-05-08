using ABO.ToDoApp.Application.Exceptions;
using ABO.ToDoApp.Domain.Repositories;
using ABO.ToDoApp.Shared.Constants.TodoLists;
using ABO.ToDoApp.Shared.Identity.Models;
using ABO.ToDoApp.Shared.Models.TodoList;
using AutoMapper;
using MediatR;

namespace ABO.ToDoApp.Application.Feautures.TodoList.Create;

public class CreateTodoListRequesHandler : IRequestHandler<CreateTodoListRequest, ActionsResponse<CreateTodoListResponse>>
{
    private readonly IMapper _mapper;
    private readonly IUnitofwork _unitofwork;
    private readonly IdentityConfig _identityConfig;

    public CreateTodoListRequesHandler(IMapper mapper, 
        IUnitofwork unitofwork, 
        IdentityConfig identityConfig)
    {
        _mapper = mapper;
        _unitofwork = unitofwork;
        _identityConfig = identityConfig;
    }

    public async Task<ActionsResponse<CreateTodoListResponse>> Handle(CreateTodoListRequest request, CancellationToken cancellationToken)
    {
        var validator = new CreateTodoListRequestValidator();
        var validatorResult = await validator.ValidateAsync(request, cancellationToken);

        if (validatorResult.Errors.Count != 0)
            throw new BadRequestException(TodoListMessageConstants.ErrorMessage, validatorResult);

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

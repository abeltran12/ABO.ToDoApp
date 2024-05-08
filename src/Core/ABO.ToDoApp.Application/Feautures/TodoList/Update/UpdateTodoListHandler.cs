using ABO.ToDoApp.Application.Exceptions;
using ABO.ToDoApp.Domain.Entities;
using ABO.ToDoApp.Domain.Repositories;
using ABO.ToDoApp.Shared.Constants.TodoLists;
using AutoMapper;
using MediatR;

namespace ABO.ToDoApp.Application.Feautures.TodoList.Update;

public class UpdateTodoListHandler : IRequestHandler<UpdateTodoListRequest, string>
{
    private readonly IMapper _mapper;
    private readonly IUnitofwork _unitofwork;

    public UpdateTodoListHandler(IMapper mapper, IUnitofwork unitofwork)
    {
        _mapper = mapper;
        _unitofwork = unitofwork;
    }

    public async Task<string> Handle(UpdateTodoListRequest request, CancellationToken cancellationToken)
    {
        var validator = new UpdateTodoListRequestValidator();
        var validatorResult = await validator.ValidateAsync(request, cancellationToken);

        if (validatorResult.Errors.Count != 0)
            throw new BadRequestException(TodoListMessageConstants.ErrorMessage, validatorResult);

        var response = await _unitofwork.TodoListRepository.GetByIdAsync(request.Id);

        if (response == null)
            throw new NotFoundException("TodoList", request.Id);

        if (response.Status.Equals(Status.Completed))
            throw new BadRequestException("Cant make modifications on a completed list.");

        _mapper.Map(request, response);
        _unitofwork.TodoListRepository.Update(response);
        await _unitofwork.SaveAsync();

        return TodoListMessageConstants.SuccessMessage;
    }
}
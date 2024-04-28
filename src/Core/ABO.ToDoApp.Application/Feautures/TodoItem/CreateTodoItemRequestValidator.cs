using ABO.ToDoApp.Domain.Entities;
using ABO.ToDoApp.Domain.Repositories;
using FluentValidation;

namespace ABO.ToDoApp.Application.Feautures.TodoItem;

public class CreateTodoItemRequestValidator : AbstractValidator<CreateTodoItemRequest>
{
    private readonly IUnitofwork _unitofwork;

    public CreateTodoItemRequestValidator(IUnitofwork unitofwork)
    {
        _unitofwork = unitofwork;

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .NotNull().WithMessage("Title cannot be null.")
            .Length(6, 100).WithMessage("Title must be between 6 and 100 characters long.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .NotNull().WithMessage("Description cannot be null.")
            .Length(6, 100).WithMessage("Description must be between 6 and 100 characters long.");

        RuleFor(x => x.Duedate)
            .NotEmpty().WithMessage("Due date cannot be empty.")
            .Must(date => date >= DateOnly.FromDateTime(DateTime.Today)).WithMessage("Due date must be greater or equal than today.");


        RuleFor(x => x.TodoListId)
                .NotEqual(0).WithMessage("Todo list ID must not be zero.")
        .MustAsync(TodoListExistsAndBelongsToUser);

    }

    private async Task<bool> TodoListExistsAndBelongsToUser(int todoListId, CancellationToken token)
    {
        return await _unitofwork.TodoListRepository.GetByIdAsync(todoListId) != null;
    }

}
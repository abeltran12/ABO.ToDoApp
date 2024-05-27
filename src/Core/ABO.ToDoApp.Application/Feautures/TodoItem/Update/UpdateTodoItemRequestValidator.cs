using ABO.ToDoApp.Domain.Entities;
using FluentValidation;

namespace ABO.ToDoApp.Application.Feautures.TodoItem.Update;

public class UpdateTodoItemRequestValidator : AbstractValidator<UpdateTodoItemRequest>
{
    public UpdateTodoItemRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .Length(6, 100).WithMessage("Title must be between 6 and 100 characters long.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .Length(6, 100).WithMessage("Description must be between 6 and 100 characters long.");

        RuleFor(x => x.Duedate)
            .NotEmpty().WithMessage("Due date cannot be empty.")
            .Must(date => date >= DateOnly.FromDateTime(DateTime.Today)).WithMessage("Due date must be greater or equal than today.");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status cannot be empty.")
            .Must(BeValidStatus)
            .WithMessage("Status must be either 1 or 2.");
    }

    private bool BeValidStatus(Status status)
    {
        return status == Status.Active || status == Status.Completed;
    }
}

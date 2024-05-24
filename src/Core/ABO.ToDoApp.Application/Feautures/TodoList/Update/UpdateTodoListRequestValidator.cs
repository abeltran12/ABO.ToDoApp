using FluentValidation;

namespace ABO.ToDoApp.Application.Feautures.TodoList.Update;

public class UpdateTodoListRequestValidator : AbstractValidator<UpdateTodoListRequest>
{
    public UpdateTodoListRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .Length(6, 100).WithMessage("Name must be between 6 and 100 characters long.");

        RuleFor(x => x.Description)
            .MaximumLength(100).WithMessage("Description must be 100 characters long or less.");
    }
}
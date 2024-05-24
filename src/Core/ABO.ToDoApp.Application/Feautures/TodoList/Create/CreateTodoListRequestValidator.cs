using FluentValidation;

namespace ABO.ToDoApp.Application.Feautures.TodoList.Create;

public class CreateTodoListRequestValidator : AbstractValidator<CreateTodoListRequest>
{
    public CreateTodoListRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .Length(6, 100).WithMessage("Name must be between 6 and 100 characters long.");

        RuleFor(x => x.Description)
            .MaximumLength(100).WithMessage("Description must be 100 characters long or less.");
    }
}
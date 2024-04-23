using FluentValidation;

namespace ABO.ToDoApp.Application.Feautures.TodoList;

public class CreateTodoListRequestValidator : AbstractValidator<CreateTodoListRequest>
{
    public CreateTodoListRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Email is required.")
            .NotNull().WithMessage("Email cannot be null.")
            .Length(6, 100).WithMessage("Name must be between 6 and 100 characters long.");
    }
}

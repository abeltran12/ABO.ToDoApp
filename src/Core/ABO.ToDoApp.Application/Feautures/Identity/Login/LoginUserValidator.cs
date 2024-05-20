using FluentValidation;

namespace ABO.ToDoApp.Application.Feautures.Identity.Login;

public class LoginUserValidator : AbstractValidator<LoginUserRequest>
{
    public LoginUserValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .Length(8, 8).WithMessage("Password length must be 8 characters.");
    }
}

﻿using FluentValidation;

namespace ABO.ToDoApp.Application.Feautures
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserRequest>
    {
        public RegisterUserValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .NotNull().WithMessage("Username cannot be null.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .NotNull().WithMessage("Email cannot be null.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .NotNull().WithMessage("Password cannot be null.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .NotNull().WithMessage("Phone number cannot be null.");
        }
    }
}

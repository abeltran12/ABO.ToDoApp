using ABO.ToDoApp.Application.Behaviors;
using ABO.ToDoApp.Application.Exceptions;
using ABO.ToDoApp.Application.Feautures.Identity.Register;
using ABO.ToDoApp.Shared.Identity.Models;
using MediatR;

namespace ABO.ToDoApp.Application.Tests.Unit.Identity.Register;

public class ValidationBehaviorTests
{
    private readonly IValidator<RegisterUserRequest> _validator = Substitute.For<IValidator<RegisterUserRequest>>();

    [Theory, AutoData]
    public async Task Handle_ShouldPassToNext_WhenValidationSucceeds(RegisterUserRequest request)
    {
        // Arrange
        _validator.Validate(Arg.Any<ValidationContext<RegisterUserRequest>>())
                 .Returns(new ValidationResult());

        var validators = new List<IValidator<RegisterUserRequest>> { _validator };
        var _sut = new ValidationBehavior<RegisterUserRequest, RegisterUserResponse>(validators);

        var next = Substitute.For<RequestHandlerDelegate<RegisterUserResponse>>();
        next().Returns(Task.FromResult(new RegisterUserResponse()));

        // Act
        var result = await _sut.Handle(request, next, CancellationToken.None);

        // Assert
        await next.Received(1).Invoke();
        result.Should().NotBeNull();
    }

    [Theory, AutoData]
    public async Task Handle_ShouldThrowValidationException_WhenValidationFails(List<ValidationFailure> failures)
    {
        // Arrange
        _validator.Validate(Arg.Any<ValidationContext<RegisterUserRequest>>())
            .Returns(new ValidationResult(failures));

        var validators = new List<IValidator<RegisterUserRequest>> { _validator };
        var _sut = new ValidationBehavior<RegisterUserRequest, RegisterUserResponse>(validators);

        var next = Substitute.For<RequestHandlerDelegate<RegisterUserResponse>>();

        // Act
        Func<Task> result = async () => await _sut.Handle(new RegisterUserRequest(), next, CancellationToken.None); ;

        // Assert
        var exception = await result.Should().ThrowAsync<ValidationAppException>()
            .WithMessage("One or more validation errors ocurred.");
    }
}

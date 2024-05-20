using ABO.ToDoApp.Application.Behaviors;
using ABO.ToDoApp.Application.Exceptions;
using ABO.ToDoApp.Application.Feautures.Identity.Login;
using MediatR;

namespace ABO.ToDoApp.Application.Tests.Unit.Identity.Login;

public class ValidationBehaviorTests
{
    private readonly IValidator<LoginUserRequest> _validator = Substitute.For<IValidator<LoginUserRequest>>();

    [Theory, AutoData]
    public async Task Handle_ShouldPassToNext_WhenValidationSucceeds(LoginUserRequest request)
    {
        // Arrange
        _validator.Validate(Arg.Any<ValidationContext<LoginUserRequest>>())
                 .Returns(new ValidationResult());

        var validators = new List<IValidator<LoginUserRequest>> { _validator };
        var _sut = new ValidationBehavior<LoginUserRequest, TokenResponse>(validators);

        var next = Substitute.For<RequestHandlerDelegate<TokenResponse>>();
        next().Returns(Task.FromResult(new TokenResponse()));

        // Act
        var result = await _sut.Handle(request, next, CancellationToken.None);

        // Assert
        await next.Received(1).Invoke();
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_ShouldThrowValidationException_WhenValidationFails()
    {
        // Arrange
        var failures = ValidationHelper.GetRandomFailures();
        _validator.Validate(Arg.Any<ValidationContext<LoginUserRequest>>())
            .Returns(new ValidationResult(failures));

        var validators = new List<IValidator<LoginUserRequest>> { _validator };
        var _sut = new ValidationBehavior<LoginUserRequest, TokenResponse>(validators);

        var next = Substitute.For<RequestHandlerDelegate<TokenResponse>>();

        // Act
        Func<Task> result = async () => await _sut.Handle(new LoginUserRequest(), next, CancellationToken.None); ;

        // Assert
        var exception = await result.Should().ThrowAsync<ValidationAppException>()
            .WithMessage("One or more validation errors ocurred.");

        foreach (var failure in failures)
        {
            exception.Which.Errors.Should().ContainKey(failure.PropertyName);
            exception.Which.Errors![failure.PropertyName].Should().Contain(failure.ErrorMessage);
        }
    }
}
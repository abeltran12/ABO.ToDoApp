using ABO.ToDoApp.Application.Behaviors;
using ABO.ToDoApp.Application.Exceptions;
using ABO.ToDoApp.Application.Tests.Unit.Behaviors.Helpers;
using MediatR;

namespace ABO.ToDoApp.Application.Tests.Unit.Behaviors;

public class ValidationBehaviorTests
{
    private readonly IValidator<SampleRequest> _validator = Substitute.For<IValidator<SampleRequest>>();

    [Theory, AutoData]
    public async Task Handle_ShouldProceed_WhenRequestIsValid(SampleRequest request)
    {
        // Arrange
        _validator.Validate(Arg.Any<ValidationContext<SampleRequest>>())
                 .Returns(new ValidationResult());

        var validators = new List<IValidator<SampleRequest>> { _validator };
        var _sut = new ValidationBehavior<SampleRequest, SampleResponse>(validators);

        var next = Substitute.For<RequestHandlerDelegate<SampleResponse>>();
        next().Returns(Task.FromResult(new SampleResponse()));

        // Act
        var result = await _sut.Handle(request, next, CancellationToken.None);

        // Assert
        await next.Received(1).Invoke();
        result.Should().NotBeNull();
    }

    [Theory, AutoData]
    public async Task Handle_ShouldThrowValidationAppException_WhenRequestIsInvalid(List<ValidationFailure> failures)
    {
        // Arrange
        _validator.Validate(Arg.Any<ValidationContext<SampleRequest>>())
            .Returns(new ValidationResult(failures));

        var validators = new List<IValidator<SampleRequest>> { _validator };
        var _sut = new ValidationBehavior<SampleRequest, SampleResponse>(validators);

        var next = Substitute.For<RequestHandlerDelegate<SampleResponse>>();

        // Act
        Func<Task> result = async () => await _sut.Handle(new SampleRequest(), next, CancellationToken.None); ;

        // Assert
        var exception = await result.Should().ThrowAsync<ValidationAppException>()
            .WithMessage("One or more validation errors ocurred.");
    }
}
using ABO.ToDoApp.Application.Behaviors;
using ABO.ToDoApp.Application.Exceptions;
using ABO.ToDoApp.Application.Feautures.TodoList.Update;
using ABO.ToDoApp.Shared.Models.TodoList;
using MediatR;

namespace ABO.ToDoApp.Application.Tests.Unit.TodoLists.Update;

public class ValidationBehaviorTests
{
    private readonly IValidator<UpdateTodoListRequest> _validator = Substitute.For<IValidator<UpdateTodoListRequest>>();

    [Theory, AutoData]
    public async Task Handle_ShouldPassToNext_WhenValidationSucceeds(UpdateTodoListRequest request)
    {
        // Arrange
        _validator.Validate(Arg.Any<ValidationContext<UpdateTodoListRequest>>())
                 .Returns(new ValidationResult());

        var validators = new List<IValidator<UpdateTodoListRequest>> { _validator };
        var _sut = new ValidationBehavior<UpdateTodoListRequest, string>(validators);

        var next = Substitute.For<RequestHandlerDelegate<string>>();
        next().Returns(Task.FromResult(""));

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
        _validator.Validate(Arg.Any<ValidationContext<UpdateTodoListRequest>>())
            .Returns(new ValidationResult(failures));

        var validators = new List<IValidator<UpdateTodoListRequest>> { _validator };
        var _sut = new ValidationBehavior<UpdateTodoListRequest, string>(validators);

        var next = Substitute.For<RequestHandlerDelegate<string>>();

        // Act
        Func<Task> result = async () => await _sut.Handle(new UpdateTodoListRequest(), next, CancellationToken.None); ;

        // Assert
        var exception = await result.Should().ThrowAsync<ValidationAppException>()
            .WithMessage("One or more validation errors ocurred.");
    }
}

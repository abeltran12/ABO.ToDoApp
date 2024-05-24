using ABO.ToDoApp.Application.Behaviors;
using ABO.ToDoApp.Application.Exceptions;
using ABO.ToDoApp.Application.Feautures.TodoList.Create;
using ABO.ToDoApp.Shared.Models.TodoList;
using MediatR;

namespace ABO.ToDoApp.Application.Tests.Unit.TodoLists.Create;

public class ValidationBehaviorTests
{
    private readonly IValidator<CreateTodoListRequest> _validator = Substitute.For<IValidator<CreateTodoListRequest>>();

    [Theory, AutoData]
    public async Task Handle_ShouldPassToNext_WhenValidationSucceeds(CreateTodoListRequest request)
    {
        // Arrange
        _validator.Validate(Arg.Any<ValidationContext<CreateTodoListRequest>>())
                 .Returns(new ValidationResult());

        var validators = new List<IValidator<CreateTodoListRequest>> { _validator };
        var _sut = new ValidationBehavior<CreateTodoListRequest, ActionsResponse<CreateTodoListResponse>> (validators);

        var next = Substitute.For<RequestHandlerDelegate<ActionsResponse<CreateTodoListResponse>>>();
        next().Returns(Task.FromResult(new ActionsResponse<CreateTodoListResponse>()));

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
        _validator.Validate(Arg.Any<ValidationContext<CreateTodoListRequest>>())
            .Returns(new ValidationResult(failures));

        var validators = new List<IValidator<CreateTodoListRequest>> { _validator };
        var _sut = new ValidationBehavior<CreateTodoListRequest, ActionsResponse<CreateTodoListResponse>>(validators);

        var next = Substitute.For<RequestHandlerDelegate<ActionsResponse<CreateTodoListResponse>>>();

        // Act
        Func<Task> result = async () => await _sut.Handle(new CreateTodoListRequest(), next, CancellationToken.None); ;

        // Assert
        var exception = await result.Should().ThrowAsync<ValidationAppException>()
            .WithMessage("One or more validation errors ocurred.");
    }
}
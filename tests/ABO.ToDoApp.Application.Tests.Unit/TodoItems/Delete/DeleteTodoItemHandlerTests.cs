using ABO.ToDoApp.Application.Exceptions;
using ABO.ToDoApp.Application.Feautures.TodoItem.Delete;
using ABO.ToDoApp.Shared.Constants.TodoItems;
using NSubstitute.ExceptionExtensions;

namespace ABO.ToDoApp.Application.Tests.Unit.TodoItems.Delete;

public class DeleteTodoItemHandlerTests
{
    private readonly DeleteTodoItemHandler _sut;
    private readonly IUnitofwork _unitofwork = Substitute.For<IUnitofwork>();

    public DeleteTodoItemHandlerTests()
	{
        _sut = new DeleteTodoItemHandler(_unitofwork);
	}

    [Theory, AutoData]
    public async Task Handle_ShouldInteractOnceWithRepository_WhenInvoked(
        DeleteTodoItemRequest request,
        CancellationToken cancellationToken)
    {
        // Arrange
        TodoItem mappedEntity = new()
        {
            Id = 1,
            Status = Status.Deleted,
            TodoListId = 1
        };

        _unitofwork.TodoItemRepository.GetByIdAsync(request.TodolistId, request.Id).Returns(mappedEntity);

        // Act
        await _sut.Handle(request, cancellationToken);

        // Assert
        _unitofwork.TodoItemRepository.Received(1).Update(mappedEntity);
        await _unitofwork.Received(1).SaveAsync();
    }

    [Theory, AutoData]
    public async Task Handle_ShouldReturnSuccessMessage_WhenInvoked(
        DeleteTodoItemRequest request,
        CancellationToken cancellationToken)
    {
        // Arrange
        TodoItem mappedEntity = new()
        {
            Id = 1,
            Status = Status.Deleted,
            TodoListId = 1
        };

        _unitofwork.TodoItemRepository.GetByIdAsync(request.TodolistId, request.Id).Returns(mappedEntity);

        // Act
        var response = await _sut.Handle(request, cancellationToken);

        // Assert
        response.Should().NotBeNull();
        response.Should().Be(TodoItemMessageConstants.SuccessMessage);
    }

    [Theory, AutoData]
    public async Task Handle_ShouldThrownNotFoundException_WhenListNotExists(
        DeleteTodoItemRequest request,
        CancellationToken cancellationToken)
    {
        // Arrange
        _unitofwork.TodoItemRepository.GetByIdAsync(request.TodolistId, request.Id).
            ThrowsAsync(new NotFoundException("TodoItem", request.Id));

        // Act
        Func<Task> result = async () => await _sut.Handle(request, cancellationToken);

        // Assert
        await result.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"The TodoItem with key ({request.Id}) doesnt exist.");
    }
}
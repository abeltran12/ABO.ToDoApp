using ABO.ToDoApp.Application.Exceptions;
using ABO.ToDoApp.Application.Feautures.TodoList.Delete;
using ABO.ToDoApp.Shared.Constants.TodoLists;
using NSubstitute.ExceptionExtensions;

namespace ABO.ToDoApp.Application.Tests.Unit.TodoLists.Delete;

public class DeleteTodoListHandlerTests
{
    private readonly DeleteTodoListHandler _sut;
    private readonly IUnitofwork _unitofwork = Substitute.For<IUnitofwork>();

    public DeleteTodoListHandlerTests()
    {
        _sut = new DeleteTodoListHandler(_unitofwork);
    }

    [Theory, AutoData]
    public async Task Handle_ShouldInteractOnceWithRepository_WhenInvoked(
        DeleteTodoListRequest request,
        CancellationToken cancellationToken)
    {
        // Arrange
        TodoList mappedEntity = new()
        {
            Id = 1,
            Status = Status.Deleted,
            UserId = "1",
            TodoItems = null
        };

        _unitofwork.TodoListRepository.GetByIdAsync(request.Id).Returns(mappedEntity);
        _unitofwork.TodoItemRepository.GetAllForUpdate(request.Id).Returns([]);

        // Act
        await _sut.Handle(request, cancellationToken);

        // Assert
        _unitofwork.TodoListRepository.Received(1).Update(mappedEntity);
        await _unitofwork.Received(1).SaveAsync();
    }

    [Theory, AutoData]
    public async Task Handle_ShouldReturnSuccessMessage_WhenInvoked(
        DeleteTodoListRequest request,
        CancellationToken cancellationToken)
    {
        // Arrange
        TodoList mappedEntity = new()
        {
            Id = 1,
            Status = Status.Deleted,
            UserId = "1",
            TodoItems = null
        };

        _unitofwork.TodoListRepository.GetByIdAsync(request.Id).Returns(mappedEntity);
        _unitofwork.TodoItemRepository.GetAllForUpdate(request.Id).Returns([]);

        // Act
        var response = await _sut.Handle(request, cancellationToken);

        // Assert
        response.Should().NotBeNull();
        response.Should().Be(TodoListMessageConstants.SuccessMessage);
    }

    [Theory, AutoData]
    public async Task Handle_ShouldInteractWithRepositoryOnceToUpdateTodoItems_WhenInvoked(
        DeleteTodoListRequest request,
        CancellationToken cancellationToken)
    {
        // Arrange
        List<TodoItem> todoItems =
        [
            new(){ Id = 1, Duedate = DateOnly.FromDateTime(DateTime.Now), Status = Status.Active, TodoListId = 1},
            new(){ Id = 2, Duedate = DateOnly.FromDateTime(DateTime.Now), Status = Status.Active, TodoListId = 1}
        ];

        TodoList mappedEntity = new()
        {
            Id = 1,
            Status = Status.Deleted,
            UserId = "1",
            TodoItems = todoItems
        };

        _unitofwork.TodoListRepository.GetByIdAsync(request.Id).Returns(mappedEntity);
        _unitofwork.TodoItemRepository.GetAllForUpdate(request.Id).Returns(todoItems);

        // Act
        await _sut.Handle(request, cancellationToken);

        // Assert
        _unitofwork.TodoItemRepository.Received(1).UpdateAll(todoItems);
        await _unitofwork.Received(1).SaveAsync();
    }

    [Theory, AutoData]
    public async Task Handle_ShouldThrownNotFoundException_WhenListNotExists(
        DeleteTodoListRequest request,
        CancellationToken cancellationToken)
    {
        // Arrange
        _unitofwork.TodoListRepository.GetByIdAsync(request.Id).
            ThrowsAsync(new NotFoundException("TodoList", request.Id));

        // Act
        Func<Task> result = async () => await _sut.Handle(request, cancellationToken);

        // Assert
        await result.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"The TodoList with key ({request.Id}) doesnt exist.");
    }
}
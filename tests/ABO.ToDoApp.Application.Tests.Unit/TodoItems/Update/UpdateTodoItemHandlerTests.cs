using ABO.ToDoApp.Application.Exceptions;
using ABO.ToDoApp.Application.Feautures.TodoItem.Update;
using ABO.ToDoApp.Shared.Constants.TodoItems;
using NSubstitute.ExceptionExtensions;

namespace ABO.ToDoApp.Application.Tests.Unit.TodoItems.Update;

public class UpdateTodoItemHandlerTests
{
    private readonly UpdateTodoItemHandler _sut;
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly IUnitofwork _unitofwork = Substitute.For<IUnitofwork>();

    public UpdateTodoItemHandlerTests()
    {
        _sut = new UpdateTodoItemHandler(_mapper, _unitofwork);
    }

    [Theory, AutoData]
    public async Task Handle_ShouldInteractOnceWithRepository_WhenInvoked(
        CancellationToken cancellationToken)
    {
        // Arrange
        var request = new UpdateTodoItemRequest { TodoListId = 1, Id = 1, Status = Status.Active };
        var mappedEntity = new TodoItem { TodoListId = 1, Id = 1, Status = Status.Active };

        _unitofwork.TodoItemRepository.GetByIdAsync(request.TodoListId, request.Id).
            Returns(mappedEntity);

        _mapper.Map<TodoItem>(request).Returns(mappedEntity);

        // Act
        await _sut.Handle(request, cancellationToken);

        // Assert
        _unitofwork.TodoItemRepository.Received(1).Update(mappedEntity);
        await _unitofwork.Received(1).SaveAsync();
    }

    [Theory, AutoData]
    public async Task Handle_ShouldReturnSuccessMessage_WhenInvoked(
        CancellationToken cancellationToken)
    {
        // Arrange
        var request = new UpdateTodoItemRequest { TodoListId = 1, Id = 1, Status = Status.Active };
        var mappedEntity = new TodoItem { TodoListId = 1, Id = 1, Status = Status.Active };

        _unitofwork.TodoItemRepository.GetByIdAsync(request.TodoListId, request.Id).
            Returns(mappedEntity);

        _mapper.Map<TodoItem>(request).Returns(mappedEntity);

        // Act
        var response = await _sut.Handle(request, cancellationToken);

        // Assert
        response.Should().NotBeNull();
        response.Should().Be(TodoItemMessageConstants.SuccessMessage);
    }

    [Theory, AutoData]
    public async Task Handle_ShouldInteractOnceWithTodoListRepository_WhenInvokedAllTheItemsHaveCompletedStatus(
        CancellationToken cancellationToken)
    {
        // Arrange
        var request = new UpdateTodoItemRequest { TodoListId = 1, Id = 1, Status = Status.Completed };
        var mappedEntity = new TodoItem { TodoListId = 1, Id = 1, Status = Status.Active };
        TodoList todoList = new()
        {
            Id = 1,
            Name = "Name",
            Description = request.Description!,
            Status = Status.Active,
            UserId = "1",
            TodoItems = null
        };

        _unitofwork.TodoItemRepository.GetByIdAsync(request.TodoListId, request.Id).
            Returns(mappedEntity);

        _unitofwork.TodoItemRepository.GetTodoItemsCompletedCount(request.TodoListId).
            Returns(9);

        _unitofwork.TodoListRepository.GetByIdAsync(request.TodoListId).
            Returns(todoList);

        _mapper.Map<TodoItem>(request).Returns(mappedEntity);

        // Act
        await _sut.Handle(request, cancellationToken);

        // Assert
        _unitofwork.TodoListRepository.Received(1).Update(todoList);
        await _unitofwork.Received(1).SaveAsync();
    }

    [Theory, AutoData]
    public async Task Handle_ShouldThrownNotFoundException_WhenTodoItemNotExists(
        CancellationToken cancellationToken)
    {
        // Arrange
        var request = new UpdateTodoItemRequest { TodoListId = 1, Id = 1 };


        _unitofwork.TodoItemRepository.GetByIdAsync(request.TodoListId, request.Id).
            ThrowsAsync(new NotFoundException("TodoItem", request.Id));

        // Act
        Func<Task> result = async () => await _sut.Handle(request, cancellationToken);

        // Assert
        await result.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"The TodoItem with key ({request.Id}) doesnt exist.");
    }

    [Theory, AutoData]
    public async Task Handle_ShouldThrownBadRequestException_WhenTodoItemStatusIsCompleted(
        CancellationToken cancellationToken)
    {
        // Arrange
        var request = new UpdateTodoItemRequest { TodoListId = 1, Id = 1, Status = Status.Active };
        var todoItem = new TodoItem { TodoListId = 1, Id = 1, Status = Status.Completed };

        _unitofwork.TodoItemRepository.GetByIdAsync(request.TodoListId, request.Id).
            Returns(todoItem);

        // Act
        Func<Task> result = async () => await _sut.Handle(request, cancellationToken);

        // Assert
        await result.Should()
            .ThrowAsync<BadRequestException>()
            .WithMessage("Cant reopen a completed activity.");
    }
}
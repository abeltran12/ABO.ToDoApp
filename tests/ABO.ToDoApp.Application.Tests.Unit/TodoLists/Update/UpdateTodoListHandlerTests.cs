using ABO.ToDoApp.Application.Exceptions;
using ABO.ToDoApp.Application.Feautures.TodoList.Update;
using ABO.ToDoApp.Shared.Constants.TodoLists;
using NSubstitute.ExceptionExtensions;

namespace ABO.ToDoApp.Application.Tests.Unit.TodoLists.Update;

public class UpdateTodoListHandlerTests
{
    private readonly UpdateTodoListHandler _sut;
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly IUnitofwork _unitofwork = Substitute.For<IUnitofwork>();

    public UpdateTodoListHandlerTests()
    {
        _sut = new UpdateTodoListHandler(_mapper, _unitofwork);
    }

    [Theory, AutoData]
    public async Task Handle_ShouldInteractOnceWithRepository_WhenInvoked(
        UpdateTodoListRequest request,
        CancellationToken cancellationToken)
    {
        // Arrange
        TodoList mappedEntity = new()
        {
            Id = 1,
            Name = request.Name,
            Description = request.Description!,
            Status = Status.Active,
            UserId = "1",
            TodoItems = null
        };

        _unitofwork.TodoListRepository.GetByIdAsync(request.Id).Returns(mappedEntity);
        _mapper.Map<TodoList>(request).Returns(mappedEntity);

        // Act
        await _sut.Handle(request, cancellationToken);

        // Assert
        _unitofwork.TodoListRepository.Received(1).Update(mappedEntity);
        await _unitofwork.Received(1).SaveAsync();
    }

    [Theory, AutoData]
    public async Task Handle_ShouldReturnSuccessMessage_WhenInvoked(
        UpdateTodoListRequest request,
        CancellationToken cancellationToken)
    {
        // Arrange
        TodoList mappedEntity = new()
        {
            Id = 1,
            Name = request.Name,
            Description = request.Description!,
            Status = Status.Active,
            UserId = "1",
            TodoItems = null
        };

        _unitofwork.TodoListRepository.GetByIdAsync(request.Id).Returns(mappedEntity);
        _mapper.Map<TodoList>(request).Returns(mappedEntity);

        // Act
        var response = await _sut.Handle(request, cancellationToken);

        // Assert
        response.Should().NotBeNull();
        response.Should().Be(TodoListMessageConstants.SuccessMessage);
    }

    [Theory, AutoData]
    public async Task Handle_ShouldThrownNotFoundException_WhenListNotExists(
        UpdateTodoListRequest request,
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

    [Theory, AutoData]
    public async Task Handle_ShouldThrownBadRequestException_WhenListHaveCompleteStatus(
        UpdateTodoListRequest request,
        CancellationToken cancellationToken)
    {
        // Arrange
        TodoList todoList = new() { Status = Status.Completed };

        _unitofwork.TodoListRepository.GetByIdAsync(request.Id).Returns(todoList);

        // Act
        Func<Task> result = async () => await _sut.Handle(request, cancellationToken);

        // Assert
        await result.Should()
            .ThrowAsync<BadRequestException>()
            .WithMessage("Cant make modifications on a completed list.");
    }
}

using ABO.ToDoApp.Application.Exceptions;
using ABO.ToDoApp.Application.Feautures.TodoItem.Create;
using ABO.ToDoApp.Shared.Constants.TodoItems;
using ABO.ToDoApp.Shared.Models.TodoItem;

namespace ABO.ToDoApp.Application.Tests.Unit.TodoItems.Create;

public class CreateTodoItemHandlerTests
{
    private readonly CreateTodoItemHandler _sut;
    private readonly CreateTodoItemRequest _request;
    private readonly TodoItem _mappedEntity;
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly IUnitofwork _unitofwork = Substitute.For<IUnitofwork>();

    public CreateTodoItemHandlerTests()
    {
        _sut = new CreateTodoItemHandler(_mapper, _unitofwork);
        _request = new CreateTodoItemRequest
        {
            TodoListId = 1,
            Title = "title",
            Description = "description",
            Duedate = DateOnly.FromDateTime(DateTime.Now)
        };

        _mappedEntity = new TodoItem
        {
            Id = 1,
            Status = Status.Active,
            TodoListId = _request.TodoListId,
            Title = _request.Title,
            Description = _request.Description,
            Duedate = _request.Duedate,
        };
    }

    [Theory, AutoData]
    public async Task Handle_ShouldInteractOnceWithRepository_WhenInvoked(
        CancellationToken cancellationToken)
    {
        // Arrange
        _mapper.Map<TodoItem>(_request).Returns(_mappedEntity);

        // Act
        await _sut.Handle(_request, cancellationToken);

        // Assert
        await _unitofwork.TodoItemRepository.Received(1).CreateAsync(_mappedEntity);
        await _unitofwork.Received(1).SaveAsync();
    }

    [Theory, AutoData]
    public async Task Handle_ShouldReturnCreateTodoListResponse_WhenInvoked(
        CancellationToken cancellationToken)
    {
        // Arrange
        var createTodoItemResponse = new CreateTodoItemResponse
        {
            Id = _mappedEntity.Id,
            Title = _mappedEntity.Title,
            Description = _mappedEntity.Description,
            Duedate = _mappedEntity.Duedate
        };

        _mapper.Map<TodoItem>(_request).Returns(_mappedEntity);
        _mapper.Map<CreateTodoItemResponse>(_mappedEntity).Returns(createTodoItemResponse);

        // Act
        var response = await _sut.Handle(_request, cancellationToken);

        // Assert
        response.Data.Should().NotBeNull();
        response.Data.Should().Be(createTodoItemResponse);
        response.Message.Should().NotBeEmpty();
        response.Message.Should().Be(TodoItemMessageConstants.TodoItemCreatedSuccessfully);
    }

    [Theory, AutoData]
    public async Task Handle_ShouldThrownBadRequestException_WhenMaximunItemsReached(
        CancellationToken cancellationToken)
    {
        // Arrange
        _unitofwork.TodoItemRepository.GetTodoItemsCount(_request.TodoListId).Returns(10);

        // Act
        Func<Task> result = async () => await _sut.Handle(_request, cancellationToken);

        // Assert
        await result.Should()
            .ThrowAsync<BadRequestException>()
            .WithMessage("The maximum allowed number of items has been reached.");
    }
}
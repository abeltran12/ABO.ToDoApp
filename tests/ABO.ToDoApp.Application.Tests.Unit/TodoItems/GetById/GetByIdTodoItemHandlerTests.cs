using ABO.ToDoApp.Application.Exceptions;
using ABO.ToDoApp.Application.Feautures.TodoItem.GetById;
using ABO.ToDoApp.Shared.Models.TodoItem;
using NSubstitute.ExceptionExtensions;

namespace ABO.ToDoApp.Application.Tests.Unit.TodoItems.GetById;

public class GetByIdTodoItemHandlerTests
{
    private readonly GetByIdTodoItemHandler _sut;
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly IUnitofwork _unitofwork = Substitute.For<IUnitofwork>();

    public GetByIdTodoItemHandlerTests()
    {
        _sut = new GetByIdTodoItemHandler(_mapper, _unitofwork);
    }

    [Theory, AutoData]
    public async Task Handle_ShouldReturnGetByIdTodoTodoItemResponse_WhenTodoItemExists(
        GetByIdTodoItemRequest request,
        CancellationToken cancellationToken)
    {
        // Arrange
        TodoItem todoItem = new()
        {
            Id = request.Id,
            Title = "test",
            Description = "Description",
            Status = Status.Active,
            Duedate = DateOnly.FromDateTime(DateTime.Now),
            TodoListId = 1
        };


        var mappedResponse = new GetByIdTodoItemResponse
        {
            Id = todoItem.Id,
            Title = todoItem.Title,
            Status = StatusHelper.GetStatusString(Status.Active),
            Description = todoItem.Description,
            TodoListId = todoItem.TodoListId,
            Duedate = todoItem.Duedate
        };

        _unitofwork.TodoItemRepository.GetByIdAsync(request.TodolistId, request.Id).Returns(todoItem);
        _mapper.Map<GetByIdTodoItemResponse>(todoItem).Returns(mappedResponse);

        // Act
        var result = await _sut.Handle(request, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(mappedResponse);
    }

    [Theory, AutoData]
    public async Task Handle_ShouldThrownNotFoundException_WhenTodoItemNotExists(
        GetByIdTodoItemRequest request,
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
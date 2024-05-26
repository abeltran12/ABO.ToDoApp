using ABO.ToDoApp.Application.Feautures.TodoItem.Get;
using ABO.ToDoApp.Shared.Models.TodoItem;

namespace ABO.ToDoApp.Application.Tests.Unit.TodoItems.Get;

public class GetTodoItemHandlerTests
{
    private readonly GetTodoItemHandler _sut;
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly IUnitofwork _unitofwork = Substitute.For<IUnitofwork>();

    public GetTodoItemHandlerTests()
    {
        _sut = new GetTodoItemHandler(_mapper, _unitofwork);
    }

    [Theory, AutoData]
    public async Task Handle_ShouldReturnEmptyList_WhenNoDataExists(
        GetTodoItemRequest request)
    {
        // Arrange

        var todoItems = new List<TodoItemSelect> { };

        _unitofwork.TodoItemRepository.GetAll(request.TodolistId)
            .Returns(todoItems);

        // Act
        var result = await _sut.Handle(request, CancellationToken.None);

        // Assert
        result.Should().BeNullOrEmpty();
    }

    [Theory, AutoData]
    public async Task Handle_ShouldReturnMappedResponses_WhenCalled(
        GetTodoItemRequest request)
    {
        // Arrange
        var todoItems = new List<TodoItemSelect> 
        { 
            new() 
            { 
                Id = 1,
                Title = "tests",
                Status = Status.Active,
                StatusDecription = StatusHelper.GetStatusString(Status.Active),
                TodoListId = request.TodolistId
            } 
        };

        _unitofwork.TodoItemRepository.GetAll(request.TodolistId).Returns(todoItems);

        var mappedResponse = new List<GetTodoItemsListResponse>
        {
            new()
            {
                Id = 1,
                Title = "tests",
                Status = "Active",
            }
        };

        _mapper.Map<IEnumerable<GetTodoItemsListResponse>>(todoItems).Returns(mappedResponse);

        // Act
        var result = await _sut.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().BeSameAs(mappedResponse);
    }
}
using ABO.ToDoApp.Application.Feautures.TodoList.Get;
using ABO.ToDoApp.Domain.RequestFilters;
using ABO.ToDoApp.Shared.Models.TodoList;

namespace ABO.ToDoApp.Application.Tests.Unit.TodoLists.Get;

public class GetTodoListHandlerTests
{
    private readonly GetTodoListHandler _sut;
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly IUnitofwork _unitofwork = Substitute.For<IUnitofwork>();

    public GetTodoListHandlerTests()
    {
        _sut = new GetTodoListHandler(_mapper, _unitofwork);
    }

    [Fact]
    public async Task Handle_ShouldReturnMappedResponsesAndMetaData_WhenCalled()
    {
        // Arrange
        var request = new GetTodoListRequest
        {
            Parameters = new TodoListParameters
            {
                PageNumber = 1,
                PageSize = 10,
                Name = null,
                Status = Status.Active
            }
        };

        var todoList = new List<TodoListSelect>
        {
            new() 
            {
                Id = 1,
                Name = "Prueba",
                Status = Status.Active,
                StatusDecription =  StatusHelper.GetStatusString(Status.Active),
                UserId = "1"
            }
        };

        var metaData = new MetaData
        {
            PageSize = 10,
            CurrentPage = 1,
            TotalCount = 1,
            TotalPages = 1,
        };

        var pagedList = new PagedList<TodoListSelect>(todoList, todoList.Count, 1, 10)
        {
            MetaData = metaData
        };

        _unitofwork.TodoListRepository.GetAll(request.Parameters).Returns(pagedList);

        var mappedResponse = new List<GetAllTodoListResponse>
        {
            new() 
            {
                Id = 1,
                Name = "Prueba",
                Status = "Active"
            }
        };

        _mapper.Map<IEnumerable<GetAllTodoListResponse>>(pagedList)
            .Returns(mappedResponse);

        // Act
        var result = await _sut.Handle(request, CancellationToken.None);

        // Assert
        result.todoListResponses.Should().NotBeNullOrEmpty();
        result.metaData.Should().NotBeNull();
        result.todoListResponses.Should().BeSameAs(mappedResponse);
        result.metaData.Should().Be(metaData);
        result.metaData.PageSize.Should().Be(request.Parameters.PageSize);
        result.metaData.CurrentPage.Should().Be(request.Parameters.PageNumber);
        result.metaData.HasNext.Should().BeFalse();
        result.metaData.HasPrevious.Should().BeFalse();
    }

    [Theory, AutoData]
    public async Task Handle_ShouldReturnEmptyListAndMetaData_WhenNoDataExists(
        GetTodoListRequest request)
    {
        // Arrange

        var todoList = new List<TodoListSelect> { };
        var metaData = new MetaData { };

        var pagedList = new PagedList<TodoListSelect>(todoList, todoList.Count, 1, 10)
        {
            MetaData = metaData
        };

        _unitofwork.TodoListRepository.GetAll(request.Parameters)
            .Returns(pagedList);

        // Act
        var result = await _sut.Handle(request, CancellationToken.None);

        // Assert
        result.todoListResponses.Should().BeNullOrEmpty();
        result.metaData.Should().NotBeNull();
        result.metaData.Should().Be(metaData);
    }
}
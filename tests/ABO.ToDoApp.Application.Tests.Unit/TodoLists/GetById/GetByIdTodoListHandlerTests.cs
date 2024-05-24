using ABO.ToDoApp.Application.Exceptions;
using ABO.ToDoApp.Application.Feautures.TodoList.GetById;
using ABO.ToDoApp.Shared.Models.TodoList;
using NSubstitute.ExceptionExtensions;

namespace ABO.ToDoApp.Application.Tests.Unit.TodoLists.GetById;

public class GetByIdTodoListHandlerTests
{
    private readonly GetByIdTodoListHandler _sut;
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly IUnitofwork _unitofwork = Substitute.For<IUnitofwork>();

    public GetByIdTodoListHandlerTests()
    {
        _sut = new GetByIdTodoListHandler(_mapper, _unitofwork);
    }

    [Theory, AutoData]
    public async Task Handle_ShouldReturnGetByIdTodoListResponse_WhenListExists(
        GetByIdTodoListRequest request,
        CancellationToken cancellationToken)
    {
        // Arrange
        TodoList todoList = new()
            {
                Id = request.Id,
                Name = "example",
                Description = "Description",
                Status = Status.Active,
                UserId = "1",
                TodoItems = null
            };


        var mappedResponse = new GetByIdTodoListResponse
        {
            Id = todoList.Id,
            Name = todoList.Name,
            Status = StatusHelper.GetStatusString(Status.Active),
            Description = todoList.Description
        };

        _unitofwork.TodoListRepository.GetByIdAsync(request.Id).Returns(todoList);
        _mapper.Map<GetByIdTodoListResponse>(todoList).Returns(mappedResponse);

        // Act
        var result = await _sut.Handle(request, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(mappedResponse);
    }

    [Theory, AutoData]
    public async Task Handle_ShouldThrownNotFoundException_WhenListNotExists(
        GetByIdTodoListRequest request, 
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

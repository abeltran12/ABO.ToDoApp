using ABO.ToDoApp.Application.Feautures.TodoList.Create;
using ABO.ToDoApp.Shared.Constants.TodoLists;
using ABO.ToDoApp.Shared.Identity.Models;
using ABO.ToDoApp.Shared.Models.TodoList;
using NSubstitute.ReceivedExtensions;

namespace ABO.ToDoApp.Application.Tests.Unit.TodoLists.Create;

public class CreateTodoListRequesHandlerTests
{
    private readonly CreateTodoListRequesHandler _sut;
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly IUnitofwork _unitofwork = Substitute.For<IUnitofwork>();
    private readonly IdentityConfig _identityConfig = Substitute.For<IdentityConfig>();

    public CreateTodoListRequesHandlerTests()
    {
        _identityConfig.UserId = "1";
        _sut = new CreateTodoListRequesHandler(_mapper, _unitofwork, _identityConfig);
    }

    [Theory, AutoData]
    public async Task Handle_ShouldInteractOnceWithRepository_WhenInvoked(
        CreateTodoListRequest request,
        CancellationToken cancellationToken)
    {
        // Arrange
        TodoList mappedEntity = new()
        {
            Id = 1,
            Name = request.Name,
            Description = request.Description!,
            Status = Status.Active,
            UserId = _identityConfig.UserId!,
            TodoItems = null
        };

        _mapper.Map<TodoList>(request).Returns(mappedEntity);

        // Act
        await _sut.Handle(request, cancellationToken);

        // Assert
        await _unitofwork.TodoListRepository.Received(1).CreateAsync(mappedEntity);
        await _unitofwork.Received(1).SaveAsync();
    }

    [Theory,AutoData]
    public async Task Handle_ShouldReturnCreateTodoListResponse_WhenInvoked(
        CreateTodoListRequest request, 
        CancellationToken cancellationToken, 
        CreateTodoListResponse todoListResponse)
    {
        // Arrange
        TodoList mappedEntity = new()
        {
           Id = 1,
           Name = request.Name,
           Description = request.Description!,
           Status = Status.Active,
           UserId = _identityConfig.UserId!,
           TodoItems = null
        };

        _mapper.Map<TodoList>(request).Returns(mappedEntity);
        _mapper.Map<CreateTodoListResponse>(mappedEntity).Returns(todoListResponse);

        // Act
        var response = await _sut.Handle(request, cancellationToken);

        // Assert
        response.Data.Should().NotBeNull();
        response.Data.Should().Be(todoListResponse);
        response.Message.Should().NotBeEmpty();
        response.Message.Should().Be(TodoListMessageConstants.TodoListCreatedSuccessfully);
    }
}
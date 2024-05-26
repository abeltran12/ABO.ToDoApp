using ABO.ToDoApp.Domain.Entities;
using ABO.ToDoApp.Domain.Models;
using ABO.ToDoApp.Infrastructure.Data.Extensions;
using FluentAssertions;
using NSubstitute;

namespace ABO.ToDoApp.Infrastructure.Tests.Unit.Extensions;

public class TodoListRepositoryExtensionsTests
{
    [Fact]
    public void SearchStatus_ShouldReturnsFilteredResults_WhenTodoListsMatchingStatus()
    {
        // Arrange
        var todoLists = new List<TodoListSelect>
        {
            new() { Status = Status.Completed },
            new() { Status = Status.Active }
        }.AsQueryable();

        var statusToSearch = Status.Completed;

        // Act
        var result = todoLists.SearchStatus(statusToSearch);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Count().Should().Be(1);
        result.Should().OnlyContain(item => item.Status == statusToSearch);
    }

    [Fact]
    public void SearchStatus_ShouldReturnEmptyResults_WhenNoMatchingStatus()
    {
        // Arrange
        var todoLists = new List<TodoListSelect>
        {
            new() { Status = Status.Active },
            new() { Status = Status.Active }
        }.AsQueryable();

        var statusToSearch = Status.Completed;

        // Act
        var result = todoLists.SearchStatus(statusToSearch);

        // Assert
        result.Should().BeEmpty();
        result.Should().NotBeNull();
    }

    [Fact]
    public void SearchStatus_ShouldReturnEmptyResults_WhenListIsEmpty()
    {
        // Arrange
        var todoLists = new List<TodoListSelect>().AsQueryable();

        var statusToSearch = Status.Completed;

        // Act
        var result = todoLists.SearchStatus(statusToSearch);

        // Assert
        result.Should().BeEmpty();
        result.Should().NotBeNull();
    }

    [Fact]
    public void SearchByName_ShouldReturnsFilteredResults_WhenTodoListsMatchingSearchTerm()
    {
        // Arrange
        var todoLists = new List<TodoListSelect>
        {
            new() { Name = "Task One" },
            new() { Name = "Task Two" },
            new() { Name = "Another Task" }
        }.AsQueryable();

        var searchTerm = "Task";

        // Act
        var result = todoLists.SearchByName(searchTerm);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Count().Should().Be(3);
        result.Should().OnlyContain(item => item.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void SearchByName_ShouldReturnAllResults_WhenSearchTermIsNullOrWhiteSpace()
    {
        // Arrange
        var todoLists = new List<TodoListSelect>
        {
            new() { Name = "Task One" },
            new() { Name = "Task Two" },
            new() { Name = "Another Task" }
        }.AsQueryable();

        var searchTerm = " ";

        // Act
        var result = todoLists.SearchByName(searchTerm);

        // Assert
        result.Should().BeEquivalentTo(todoLists);
    }

    [Fact]
    public void SearchByName_ShouldReturnEmptyResults_WhenNoMatchingStatus()
    {
        // Arrange
        var todoLists = new List<TodoListSelect>
        {
            new() { Name = "Task One" },
            new() { Name = "Task Two" },
            new() { Name = "Another Task" }
        }.AsQueryable();

        var statusToSearch = "test";

        // Act
        var result = todoLists.SearchByName(statusToSearch);

        // Assert
        result.Should().BeEmpty();
        result.Should().NotBeNull();
    }

    [Fact]
    public void SearchByName_ShouldReturnEmptyResults_WhenListIsEmpty()
    {
        // Arrange
        var todoLists = new List<TodoListSelect>().AsQueryable();

        var statusToSearch = Arg.Any<string>();

        // Act
        var result = todoLists.SearchByName(statusToSearch);

        // Assert
        result.Should().BeEmpty();
        result.Should().NotBeNull();
    }
}
using ABO.ToDoApp.Application.Feautures.TodoList.Update;
using FluentValidation.TestHelper;

namespace ABO.ToDoApp.Application.Tests.Unit.TodoLists.Update;

public class UpdateTodoListRequestValidatorTests
{
    private readonly UpdateTodoListRequestValidator _sut;

    public UpdateTodoListRequestValidatorTests()
    {
        _sut = new UpdateTodoListRequestValidator();
    }

    [Theory]
    [InlineData("")]
    public void Should_Have_Error_When_Name_Is_Empty(string name)
    {
        // Arrange
        var request = new UpdateTodoListRequest { Name = name };

        // Act
        var result = _sut.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
              .WithErrorMessage("Name is required.");
    }

    [Theory]
    [InlineData("Short")]
    public void Should_Have_Error_When_Name_Is_Too_Short(string name)
    {
        // Arrange
        var request = new UpdateTodoListRequest { Name = name };

        // Act
        var result = _sut.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
              .WithErrorMessage("Name must be between 6 and 100 characters long.");
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Too_Long()
    {
        // Arrange
        var request = new UpdateTodoListRequest { Name = new string('a', 101) };

        // Act
        var result = _sut.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
              .WithErrorMessage("Name must be between 6 and 100 characters long.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Name_Is_Valid()
    {
        // Arrange
        var request = new UpdateTodoListRequest { Name = "ValidName" };

        // Act
        var result = _sut.TestValidate(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Description_Is_Too_Long()
    {
        // Arrange
        var request = new UpdateTodoListRequest { Description = new string('a', 101) };

        // Act
        var result = _sut.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description)
              .WithErrorMessage("Description must be 100 characters long or less.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Description_Is_Valid()
    {
        // Arrange
        var request = new UpdateTodoListRequest { Description = "Valid Description" };

        // Act
        var result = _sut.TestValidate(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
    }
}
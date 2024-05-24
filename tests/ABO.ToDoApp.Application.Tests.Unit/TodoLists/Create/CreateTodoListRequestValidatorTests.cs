using ABO.ToDoApp.Application.Feautures.TodoList.Create;
using FluentValidation.TestHelper;

namespace ABO.ToDoApp.Application.Tests.Unit.TodoLists.Create;

public class CreateTodoListRequestValidatorTests
{
    private readonly CreateTodoListRequestValidator _sut;

    public CreateTodoListRequestValidatorTests()
    {
        _sut = new CreateTodoListRequestValidator();
    }

    [Theory]
    [InlineData("")]
    public void Should_Have_Error_When_Name_Is_Empty(string name)
    {
        // Arrange
        var request = new CreateTodoListRequest { Name = name };

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
        var request = new CreateTodoListRequest { Name = name };

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
        var request = new CreateTodoListRequest { Name = new string('a', 101) };

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
        var request = new CreateTodoListRequest { Name = "ValidName" };

        // Act
        var result = _sut.TestValidate(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Description_Is_Too_Long()
    {
        // Arrange
        var request = new CreateTodoListRequest { Description = new string('a', 101) };

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
        var request = new CreateTodoListRequest { Description = "Valid Description" };

        // Act
        var result = _sut.TestValidate(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
    }
}
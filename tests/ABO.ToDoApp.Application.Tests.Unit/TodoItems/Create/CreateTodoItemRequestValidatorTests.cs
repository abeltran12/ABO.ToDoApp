using ABO.ToDoApp.Application.Feautures.TodoItem.Create;
using FluentValidation.TestHelper;

namespace ABO.ToDoApp.Application.Tests.Unit.TodoItems.Create;

public class CreateTodoItemRequestValidatorTests
{
    private readonly CreateTodoItemRequestValidator _sut;

    public CreateTodoItemRequestValidatorTests()
    {
        _sut = new CreateTodoItemRequestValidator();
    }

    [Theory]
    [InlineData("")]
    public void Validator_ShouldHaveAnError_WhenTitleIsEmpty(string title)
    {
        // Arrange
        var request = new CreateTodoItemRequest { Title = title };

        // Act
        var result = _sut.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title)
              .WithErrorMessage("Title is required.");
    }

    [Theory]
    [InlineData("Short")]
    public void Validator_ShouldHaveAnError_WhenTitleIsTooShort(string title)
    {
        // Arrange
        var request = new CreateTodoItemRequest { Title = title };

        // Act
        var result = _sut.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title)
              .WithErrorMessage("Title must be between 6 and 100 characters long.");
    }

    [Fact]
    public void Validator_ShouldHaveAnError_WhenTitleIsTooLong()
    {
        // Arrange
        var request = new CreateTodoItemRequest { Title = new string('a', 101) };

        // Act
        var result = _sut.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title)
              .WithErrorMessage("Title must be between 6 and 100 characters long.");
    }

    [Fact]
    public void Validator_ShouldNotHaveError_WhenNameIsValid()
    {
        // Arrange
        var request = new CreateTodoItemRequest { Title = "ValidTitle" };

        // Act
        var result = _sut.TestValidate(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Validator_ShouldHaveAnError_WhenDescriptionIsTooLong()
    {
        // Arrange
        var request = new CreateTodoItemRequest { Description = new string('a', 101) };

        // Act
        var result = _sut.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description)
              .WithErrorMessage("Description must be between 6 and 100 characters long.");
    }

    [Fact]
    public void Validator_ShouldNotHaveError_WhenDescriptionIsValid()
    {
        // Arrange
        var request = new CreateTodoItemRequest { Description = "Valid Description" };

        // Act
        var result = _sut.TestValidate(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Validator_ShouldHaveError_WhenDueDateIsEmpty()
    {
        // Arrange
        var request = new CreateTodoItemRequest { Duedate = default };

        // Act
        var result = _sut.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Duedate)
            .WithErrorMessage("Due date cannot be empty.");
    }

    [Fact]
    public void Validator_ShouldHaveAnError_WhenDueDateIsInThePast()
    {
        // Arrange
        var request = new CreateTodoItemRequest 
        { 
            Duedate = DateOnly.FromDateTime(DateTime.Today.AddDays(-1)) 
        };

        // Act
        var result = _sut.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Duedate)
            .WithErrorMessage("Due date must be greater or equal than today.");
    }

    [Fact]
    public void Validator_ShouldNotHaveError_WhenDueDateIsInTheFuture()
    {
        // Arrange
        var request = new CreateTodoItemRequest 
        {
            Duedate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)) 
        };

        // Act
        var result = _sut.TestValidate(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Duedate);
    }
}
using ABO.ToDoApp.Application.Feautures.Identity.Login;
using FluentValidation.TestHelper;

namespace ABO.ToDoApp.Application.Tests.Unit.Identity.Login;

public class LoginUserValidatorTests
{
    private readonly LoginUserValidator _sut;

    public LoginUserValidatorTests()
    {
        _sut = new LoginUserValidator();
    }

    [Theory]
    [InlineData("")]
    public void LoginUserValidator_ShouldHaveAnError_WhenEmailIsEmpty(string email)
    {
        // Arrange
        var request = new LoginUserRequest { Email = email };

        // Act
        var result = _sut.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Email)
              .WithErrorMessage("Email is required.");
    }

    [Theory]
    [InlineData("invalid-email")]
    public void LoginUserValidator_ShouldHaveAnError_WhenEmailIsInvalid(string email)
    {
        // Arrange
        var request = new LoginUserRequest { Email = email };

        // Act
        var result = _sut.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email)
              .WithErrorMessage("Invalid email format.");
    }

    [Theory]
    [InlineData("")]
    public void LoginUserValidator_ShouldHaveAnError_WhenPasswordIsEmpty(string password)
    {
        // Arrange
        var request = new LoginUserRequest { Password = password };

        // Act
        var result = _sut.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password)
              .WithErrorMessage("Password is required.");
    }

    [Theory]
    [InlineData("1234567")]
    public void LoginUserValidator_ShouldHaveAnError_WhenPasswordIsNotOfLength8(string password)
    {
        // Arrange
        var request = new LoginUserRequest { Password = password };

        // Act
        var result = _sut.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password)
              .WithErrorMessage("Password length must be 8 characters.");
    }

    [Fact]
    public void LoginUserValidator_ShouldNotHaveAnError_WhenInvoked()
    {
        // Arrange
        var request = new LoginUserRequest { Email = "test@example.com", Password = "12345678" };

        // Act
        var result = _sut.TestValidate(request);

        // Assert
        result.Errors.Should().BeEmpty();
    }
}
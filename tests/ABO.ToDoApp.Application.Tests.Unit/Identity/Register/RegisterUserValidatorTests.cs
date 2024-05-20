using ABO.ToDoApp.Application.Feautures.Identity.Register;
using FluentValidation.TestHelper;

namespace ABO.ToDoApp.Application.Tests.Unit.Identity.Register;

public class RegisterUserValidatorTests
{
    private readonly RegisterUserValidator _sut;

    public RegisterUserValidatorTests()
    {
        _sut = new RegisterUserValidator();
    }

    [Theory]
    [InlineData("")]
    public void RegisterUserValidator_ShouldHaveError_WhenUserNameIsNullOrEmpty(string userName)
    {
        // Arrange
        var request = new RegisterUserRequest { UserName = userName };

        // Act
        var result = _sut.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserName)
              .WithErrorMessage("UserName is required.");
    }

    [Theory]
    [InlineData("")]
    public void RegisterUserValidator_ShouldHaveAnError_WhenEmailIsEmpty(string email)
    {
        // Arrange
        var request = new RegisterUserRequest { Email = email };

        // Act
        var result = _sut.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Email)
              .WithErrorMessage("Email is required.");
    }

    [Theory]
    [InlineData("invalid-email")]
    public void RegisterUserValidator_ShouldHaveAnError_WhenEmailIsInvalid(string email)
    {
        // Arrange
        var request = new RegisterUserRequest { Email = email };

        // Act
        var result = _sut.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email)
              .WithErrorMessage("Invalid email format.");
    }

    [Theory]
    [InlineData("")]
    public void RegisterUserValidator_ShouldHaveError_WhenPasswordIsNullOrEmpty(string password)
    {
        // Arrange
        var request = new RegisterUserRequest { Password = password };

        // Act
        var result = _sut.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password)
              .WithErrorMessage("Password is required.");
    }

    [Theory]
    [InlineData("short")]
    [InlineData("toolongpassword")]
    public void RegisterUserValidator_ShouldHaveError_WhenPasswordIsNotEightCharacters(string password)
    {
        // Arrange
        var request = new RegisterUserRequest { Password = password };

        // Act
        var result = _sut.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password)
              .WithErrorMessage("Password length must be 8 characters.");
    }

    [Theory]
    [InlineData("")]
    public void RegisterUserValidator_ShouldHaveError_WhenPhoneNumberIsNullOrEmpty(string phoneNumber)
    {
        // Arrange
        var request = new RegisterUserRequest { PhoneNumber = phoneNumber };

        // Act
        var result = _sut.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber)
              .WithErrorMessage("Phone number is required.");
    }

    [Fact]
    public void RegisterUserValidator_ShouldNotHaveAnError_WhenInvoked()
    {
        // Arrange
        var request = new RegisterUserRequest
        {
            UserName = "validUsername",
            Email = "test@example.com",
            Password = "password",
            PhoneNumber = "1234567890"
        };

        // Act
        var result = _sut.TestValidate(request);

        // Assert
        result.Errors.Should().BeEmpty();
    }
}
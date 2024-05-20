using ABO.ToDoApp.Application.Contracts;
using ABO.ToDoApp.Application.Exceptions;
using ABO.ToDoApp.Application.Feautures.Identity.Login;
using ABO.ToDoApp.Domain.Entities;
using ABO.ToDoApp.Infrastructure.Identity.Models;
using ABO.ToDoApp.Infrastructure.Identity.Services;
using ABO.ToDoApp.Infrastructure.Tests.Unit.Fakes;
using ABO.ToDoApp.Shared.Identity.Models;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace ABO.ToDoApp.Infrastructure.Tests.Unit;

public class AuthServiceTests
{
    private readonly AuthService _sut;
    private readonly FakeUserManager _userManager = new();
    private readonly FakeSignInManager _signInManager = new();
    private readonly IOptions<JwtConfiguration> _options = Substitute.For<IOptions<JwtConfiguration>>();
    private readonly ILoggerAdapter<AuthService> _logger = Substitute.For<ILoggerAdapter<AuthService>>();
    private readonly IDateTimeProvider _dateTimeProvider = Substitute.For<IDateTimeProvider>();

    public AuthServiceTests()
    {
        var jwtConfiguration = new JwtConfiguration
        {
            Key = "kakakakakakakakakakakakakakakakakakakakakakkskskskskssksksk",
            Expires = "30",
            Audience = "https://localhost:7290",
            Issuer = "https://localhost:7290"
        };
        _options.Value.Returns(jwtConfiguration);
        _sut = new AuthService(_userManager, _signInManager, _options, _logger, _dateTimeProvider);
    }

    [Theory, AutoData]
    public async Task RegisterUser_ShouldReturnSuccessMessage_WhenDataAreCorrect(User user, string password)
    {
        // Arrange

        // Act
        var result = await _sut.RegisterUser(user, password);

        // Assert
        result.Should().BeOfType<RegisterUserResponse>();
        result.As<RegisterUserResponse>().Should().NotBeNull();
        result.Message.Should().Be("The user was successfully registered");
    }

    [Theory, AutoData]
    public async Task RegisterUser_ShouldThrownAValidationAppException_WhenRegisterFail(User user, string password)
    {
        // Arrange
        AuthService authService =
            new(new FakeUserManager(false), _signInManager, _options, _logger, _dateTimeProvider);

        // Act
        Func<Task> result = async () => await authService.RegisterUser(user, password);

        // Assert
        await result.Should()
            .ThrowAsync<ValidationAppException>()
            .WithMessage("One or more validation errors ocurred.");
    }

    [Fact]
    public async Task RegisterUser_ShouldLogMessages_WhenInvoked()
    {
        // Arrange
        var user = new User();
        var password = "validPassword";

        // Act
        await _sut.RegisterUser(user, password);

        // Assert
        _logger.Received(1).LogInformation(Arg.Is("Executing {Request}"), Arg.Is("RegisterUser"));
        _logger.Received(1).LogInformation(Arg.Is("Executing {Request} processed succefully"), Arg.Is("RegisterUser"));
    }


    [Theory, AutoData]
    public async Task ValidateUser_ShouldLogMessages_WhenInvoked(LoginUserRequest request)
    {
        // Arrange

        // Act
        await _sut.ValidateUser(request);

        // Assert
        _logger.Received(1).LogInformation(Arg.Is("Executing {Request}"), Arg.Is("ValidateUser"));
        _logger.Received(1).LogInformation(Arg.Is("Executing {Request} processed succefully"), Arg.Is("ValidateUser"));
    }

    [Theory, AutoData]
    public async Task ValidateUser_ShouldReturnTokenResponse_WhenDataAreCorrect(LoginUserRequest request)
    {
        // Arrange
        _dateTimeProvider.UtcNow.Returns(new DateTime(2024, 5, 18, 12, 0, 0, DateTimeKind.Utc));
        var expirationToken = _dateTimeProvider.UtcNow
            .AddMinutes(Convert.ToDouble(_options.Value.Expires));

        // Act
        var result = await _sut.ValidateUser(request);

        // Assert
        result.Should().BeOfType<TokenResponse>();
        result.As<TokenResponse>().Should().NotBeNull();
        result.ExpirationDate.Should().Be(expirationToken);
    }

    [Theory, AutoData]
    public async Task ValidateUser_ShouldThrownAnNotFoundException_WhenRequestFail(LoginUserRequest request)
    {
        // Arrange
        AuthService authService =
            new(new FakeUserManager(false), _signInManager, _options, _logger, _dateTimeProvider);

        // Act
        Func<Task> result = async () => await authService.ValidateUser(request);

        // Assert
        await result.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"The email with key ({request.Email}) doesnt exist.");
    }

    [Theory, AutoData]
    public async Task ValidateUser_ShouldThrownAnBadRequestException_WhenReceivesBadInput(LoginUserRequest request)
    {
        // Arrange
        AuthService authService =
            new(_userManager, new FakeSignInManager(false), _options, _logger, _dateTimeProvider);

        // Act
        Func<Task> result = async () => await authService.ValidateUser(request);

        // Assert
        await result.Should()
            .ThrowAsync<BadRequestException>()
            .WithMessage("Authentication failed. Wrong user name or password.");
    }
}
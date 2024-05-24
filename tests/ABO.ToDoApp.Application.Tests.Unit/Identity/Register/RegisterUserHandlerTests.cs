using ABO.ToDoApp.Application.Feautures.Identity.Register;
using ABO.ToDoApp.Contracts;
using ABO.ToDoApp.Domain.Entities;
using ABO.ToDoApp.Shared.Identity.Models;
using AutoMapper;

namespace ABO.ToDoApp.Application.Tests.Unit.Identity.Register;

public class RegisterUserHandlerTests
{
    private readonly RegisterUserHandler _sut;
    private readonly IAuthService _authService = Substitute.For<IAuthService>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();

    public RegisterUserHandlerTests()
    {
        _sut = new RegisterUserHandler(_authService, _mapper);
    }

    [Theory, AutoData]
    public async Task Handle_ShouldReturnRegisterUserResponse_WhenInvoked
        (RegisterUserRequest request, CancellationToken cancellationToken, User user)
    {
        // Arrange
        _mapper.Map<User>(request).Returns(user);
        RegisterUserResponse requestResponse = new();
        _authService.RegisterUser(user, request.Password).Returns(requestResponse);

        // Act
        var response = await _sut.Handle(request, cancellationToken);

        // Assert
        response.Should().NotBeNull();
        response.Should().Be(requestResponse);
    }
}
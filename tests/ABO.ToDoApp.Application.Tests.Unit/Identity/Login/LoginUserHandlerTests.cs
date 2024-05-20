using ABO.ToDoApp.Application.Contracts;
using ABO.ToDoApp.Application.Feautures.Identity.Login;
using ABO.ToDoApp.Contracts;

namespace ABO.ToDoApp.Application.Tests.Unit.Identity.Login;

public class LoginUserHandlerTests
{
    private readonly LoginUserHandler _sut;
    private readonly IAuthService _authService = Substitute.For<IAuthService>();
    private readonly IDateTimeProvider _dateTimeProvider = Substitute.For<IDateTimeProvider>();

    public LoginUserHandlerTests()
    {
        _sut = new LoginUserHandler(_authService);
    }

    [Theory, AutoData]
    public async Task LoginUserHandler_ShouldReturnTokenResponse_WhenInvoked
        (LoginUserRequest request, CancellationToken cancellationToken, IFixture fixture)
    {
        //Arrange
        fixture.Customize<TokenResponse>(tr =>
            tr.With(r => r.Token, "dummy_token")
              .With(r => r.ExpirationDate, _dateTimeProvider.UtcNow.AddMinutes(30)));

        var tokenResponse = fixture.Create<TokenResponse>();

        _authService.ValidateUser(request).Returns(tokenResponse);

        //Act
        var response = await _sut.Handle(request, cancellationToken);

        //Assert
        response.Should().NotBeNull();
        response.Should().Be(tokenResponse);
    }
}

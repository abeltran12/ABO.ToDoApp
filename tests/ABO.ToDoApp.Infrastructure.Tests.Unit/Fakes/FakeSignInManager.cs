using ABO.ToDoApp.Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace ABO.ToDoApp.Infrastructure.Tests.Unit.Fakes;

public class FakeSignInManager : SignInManager<User>
{
    private readonly bool _shouldSucceed;

    public FakeSignInManager(bool shouldSucceed = true)
        : base(
              new FakeUserManager(),
              new HttpContextAccessor(),
              Substitute.For<IUserClaimsPrincipalFactory<User>>(),
              Substitute.For<IOptions<IdentityOptions>>(),
              Substitute.For<ILogger<SignInManager<User>>>(),
              Substitute.For<IAuthenticationSchemeProvider>(),
              Substitute.For<IUserConfirmation<User>>())
    {
        _shouldSucceed = shouldSucceed;
    }

    public override Task<SignInResult> CheckPasswordSignInAsync(User user, string password, bool lockoutOnFailure)
    {
        if (user != null && _shouldSucceed)
            return Task.FromResult(SignInResult.Success);

        return Task.FromResult(SignInResult.Failed);
    }
}
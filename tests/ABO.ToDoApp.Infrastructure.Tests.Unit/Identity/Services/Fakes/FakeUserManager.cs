using ABO.ToDoApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using System.Security.Claims;

namespace ABO.ToDoApp.Infrastructure.Tests.Unit.Identity.Services.Fakes;

public class FakeUserManager : UserManager<User>
{
    private readonly bool _shouldSucceed;

    public FakeUserManager(bool shouldSucceed = true)
        : base(
              Substitute.For<IUserStore<User>>(),
              Substitute.For<IOptions<IdentityOptions>>(),
              Substitute.For<IPasswordHasher<User>>(),
              Array.Empty<IUserValidator<User>>(),
              Array.Empty<IPasswordValidator<User>>(),
              Substitute.For<ILookupNormalizer>(),
              Substitute.For<IdentityErrorDescriber>(),
              Substitute.For<IServiceProvider>(),
              Substitute.For<ILogger<UserManager<User>>>())
    {
        _shouldSucceed = shouldSucceed;
    }

    public override Task<IdentityResult> CreateAsync(User user, string password)
    {
        if (_shouldSucceed)
            return Task.FromResult(IdentityResult.Success);

        var errors = new List<IdentityError>
            {
                new IdentityError { Code = "Error", Description = "Failed to create user" }
            };
        return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
    }

    public override Task<User?> FindByEmailAsync(string email)
    {
        if (!_shouldSucceed)
            return Task.FromResult<User?>(null);

        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            Email = email,
            UserName = "ExampleName"
        };

        return Task.FromResult<User?>(user);
    }

    public override Task<IList<Claim>> GetClaimsAsync(User user)
    {
        // Create a list of claims as needed for testing
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName!),
            new(ClaimTypes.Email, user.Email!)
        };

        // Return the list of claims wrapped in a Task
        return Task.FromResult<IList<Claim>>(claims);
    }
}
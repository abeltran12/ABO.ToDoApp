using ABO.ToDoApp.Application.Exceptions;
using ABO.ToDoApp.Application.Feautures.Identity.Login;
using ABO.ToDoApp.Contracts;
using ABO.ToDoApp.Domain.Entities;
using ABO.ToDoApp.Infrastructure.Identity.Models;
using ABO.ToDoApp.Shared.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace ABO.ToDoApp.Infrastructure.Identity.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IOptions<JwtConfiguration> _jwtSettings;
    private readonly ILogger<AuthService> _logger;

    public AuthService(UserManager<User> userManager,
            SignInManager<User> signInManager,
            IOptions<JwtConfiguration> jwtSettings,
            ILogger<AuthService> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtSettings = jwtSettings;
        _logger = logger;
    }

    public async Task<RegisterUserResponse> RegisterUser(User user, string password)
    {
        _logger.LogInformation("Executing {Request}", nameof(RegisterUser));

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
            throw new BadRequestException("Register request invalid.", result.Errors);

        _logger.LogInformation("Executing {Request} processed succefully", nameof(RegisterUser));

        return new RegisterUserResponse();
    }

    public async Task<TokenResponse> ValidateUser(LoginUserRequest request)
    {
        _logger.LogInformation("Executing {Request}", nameof(ValidateUser));

        var user = await _userManager.FindByEmailAsync(request.Email!) ?? throw new NotFoundException("email", request.Email!);
        var isSignInValid = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!isSignInValid.Succeeded)
            throw new BadRequestException("Authentication failed. Wrong user name or password.");

        var tokenResponse = await CreateToken(user);

        _logger.LogInformation("Executing {Request} processed succefully", nameof(ValidateUser));

        return tokenResponse;
    }

    private async Task<TokenResponse> CreateToken(User user)
    {
        _logger.LogInformation("Executing {Request}", nameof(CreateToken));

        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims(user);
        var expirationDate = DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings.Value.Expires));
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims, expirationDate);
        var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        _logger.LogInformation("Executing {Request} processed succefully", nameof(ValidateUser));

        return new TokenResponse { Token = accessToken, ExpirationDate = expirationDate};
    }

    private SigningCredentials GetSigningCredentials()
    {
        var key = _jwtSettings.Value.Key;
        var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!));
        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private async Task<IEnumerable<Claim>> GetClaims(User user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim("uid", user.Id)
        }
        .Union(userClaims);

        return claims;
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, 
        IEnumerable<Claim> claims, DateTime expirationDate)
    {
        var tokenOptions = new JwtSecurityToken
        (
            issuer: _jwtSettings.Value.Issuer,
            audience: _jwtSettings.Value.Audience,
            claims: claims,
            expires: expirationDate,
            signingCredentials: signingCredentials
        );

        return tokenOptions;
    }

}
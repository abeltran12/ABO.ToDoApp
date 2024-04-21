﻿using ABO.ToDoApp.Application.Exceptions;
using ABO.ToDoApp.Application.Feautures.Identity.Login;
using ABO.ToDoApp.Contracts;
using ABO.ToDoApp.Domain.Entities;
using ABO.ToDoApp.Infrastructure.Identity.Models;
using ABO.ToDoApp.Shared.Identity.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace ABO.ToDoApp.Infrastructure.Identity.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly SignInManager<User> _signInManager;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IOptions<JwtConfiguration> _jwtSettings;

    public AuthService(UserManager<User> userManager, 
            IMapper mapper, 
            SignInManager<User> signInManager, 
            IHttpContextAccessor contextAccessor,
            IOptions<JwtConfiguration> jwtSettings)
    {
        _userManager = userManager;
        _mapper = mapper;
        _signInManager = signInManager;
        _contextAccessor = contextAccessor;
        _jwtSettings = jwtSettings;
    }

    public async Task<RegisterUserResponse> RegisterUser(User user, string password)
    {
        var userExists = await _userManager.FindByEmailAsync(user.Email!);

        if (userExists != null)
            throw new BadRequestException("Email is already in use.");

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
            throw new BadRequestException("Register request invalid.", result.Errors);

        return new RegisterUserResponse();
    }

    public async Task<TokenResponse> ValidateUser(LoginUserRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email!) ?? throw new NotFoundException("email", request.Email!);
        var isSignInValid = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!isSignInValid.Succeeded)
            throw new BadRequestException("Authentication failed. Wrong user name or password.");

        var tokenResponse = await CreateToken(user);

        return tokenResponse;
    }

    private async Task<TokenResponse> CreateToken(User user)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims(user);
        var expirationDate = DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings.Value.Expires));
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims, expirationDate);
        var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
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
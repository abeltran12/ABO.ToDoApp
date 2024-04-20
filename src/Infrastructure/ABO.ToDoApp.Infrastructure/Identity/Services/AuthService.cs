using ABO.ToDoApp.Application.Exceptions;
using ABO.ToDoApp.Application.Feautures.Identity.Login;
using ABO.ToDoApp.Contracts;
using ABO.ToDoApp.Domain.Entities;
using ABO.ToDoApp.Shared.Identity.Models;
using AutoMapper;
using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace ABO.ToDoApp.Infrastructure.Identity.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private User? _user;

    public AuthService(UserManager<User> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
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

    public async Task<bool> ValidateUser(LoginUserRequest request)
    {
        _user = await _userManager.FindByEmailAsync(request.Email!);

        if (_user is null)
            throw new NotFoundException("email", request.Email!);

        var result = await _userManager.CheckPasswordAsync(_user, request.Password);

        //colocar un log

        return result;
    }

    public Task<TokenResponse> CreateToken()
    {
        throw new NotImplementedException();
    }

}
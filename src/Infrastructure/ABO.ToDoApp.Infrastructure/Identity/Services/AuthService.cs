using ABO.ToDoApp.Application.Exceptions;
using ABO.ToDoApp.Application.Feautures;
using ABO.ToDoApp.Application.Feautures.Identity;
using ABO.ToDoApp.Contracts;
using ABO.ToDoApp.Domain.Entities;
using ABO.ToDoApp.Shared.Identity.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;


namespace ABO.ToDoApp.Infrastructure.Identity.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;

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

        return new RegisterUserResponse(user.Id);
    }

    //public Task<Result<bool>> ValidateUser(UserForAuthenticationRequest request)
    //{
    //    throw new NotImplementedException();
    //}
}
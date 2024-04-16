using ABO.ToDoApp.Application;
using ABO.ToDoApp.Application.Exceptions;
using ABO.ToDoApp.Application.Feautures;
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

    public async Task<RegisterUserResponse> RegisterUser(RegisterUserRequest request)
    {
        var validator = new RegisterUserValidator();
        var validatorResult = await validator.ValidateAsync(request);

        if (validatorResult.Errors.Count != 0)
            throw new BadRequestException("Register request invalid.", validatorResult);

        var user = await _userManager.FindByEmailAsync(request.Email!);

        if (user != null)
            throw new BadRequestException("Email is already in use.");

        var userForCreation = _mapper.Map<User>(request);

        var result = await _userManager.CreateAsync(userForCreation, request.Password);

        if (!result.Succeeded)
            throw new BadRequestException("Register request invalid.", result.Errors);

        return new RegisterUserResponse(userForCreation.Id);
    }

    //public Task<Result<bool>> ValidateUser(UserForAuthenticationRequest request)
    //{
    //    throw new NotImplementedException();
    //}
}